using AutoMapper;
using ContactCalls.Domain.Entities;
using ContactCalls.Dtos;
using ContactCalls.Infrastructure;
using ContactCalls.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactCalls.Services.Implementations;

public class CallService : ICallService
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;

	public CallService(ApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IEnumerable<CallDto>> GetAllCallsAsync()
	{
		var calls = await _context.Calls
			.Include(c => c.FromPhone)
				.ThenInclude(p => p.Contact)
			.Include(c => c.ToPhone)
				.ThenInclude(p => p.Contact)
			.OrderByDescending(c => c.StartTime)
			.ToListAsync();

		return _mapper.Map<IEnumerable<CallDto>>(calls);
	}

	public async Task<CallDto?> GetCallByIdAsync(int id)
	{
		var call = await _context.Calls
			.Include(c => c.FromPhone)
				.ThenInclude(p => p.Contact)
			.Include(c => c.ToPhone)
				.ThenInclude(p => p.Contact)
			.FirstOrDefaultAsync(c => c.Id == id);

		return call == null ? null : _mapper.Map<CallDto>(call);
	}

	public async Task<IEnumerable<CallDto>> GetCallsByPhoneIdAsync(int phoneId)
	{
		var calls = await _context.Calls
			.Include(c => c.FromPhone)
				.ThenInclude(p => p.Contact)
			.Include(c => c.ToPhone)
				.ThenInclude(p => p.Contact)
			.Where(c => c.FromPhoneId == phoneId || c.ToPhoneId == phoneId)
			.OrderByDescending(c => c.StartTime)
			.ToListAsync();

		return _mapper.Map<IEnumerable<CallDto>>(calls);
	}

	public async Task<IEnumerable<CallDto>> GetCallsByContactIdAsync(int contactId)
	{
		var calls = await _context.Calls
			.Include(c => c.FromPhone)
				.ThenInclude(p => p.Contact)
			.Include(c => c.ToPhone)
				.ThenInclude(p => p.Contact)
			.Where(c => c.FromPhone.ContactId == contactId || c.ToPhone.ContactId == contactId)
			.OrderByDescending(c => c.StartTime)
			.ToListAsync();

		return _mapper.Map<IEnumerable<CallDto>>(calls);
	}

	public async Task<IEnumerable<CallDto>> GetCallsByDateRangeAsync(DateTime startDate, DateTime endDate)
	{
		var calls = await _context.Calls
			.Include(c => c.FromPhone)
				.ThenInclude(p => p.Contact)
			.Include(c => c.ToPhone)
				.ThenInclude(p => p.Contact)
			.Where(c => c.StartTime >= startDate && c.StartTime <= endDate)
			.OrderByDescending(c => c.StartTime)
			.ToListAsync();

		return _mapper.Map<IEnumerable<CallDto>>(calls);
	}

	public async Task<CallDto> CreateCallAsync(CreateCallDto dto)
	{
		var fromPhone = await _context.Phones.FindAsync(dto.FromPhoneId);
		var toPhone = await _context.Phones.FindAsync(dto.ToPhoneId);

		if (fromPhone == null || toPhone == null)
			throw new ArgumentException("One or both phones not found");

		if (dto.FromPhoneId == dto.ToPhoneId)
			throw new ArgumentException("Cannot call the same phone");

		var call = _mapper.Map<Call>(dto);

		if (dto.EndTime.HasValue && dto.EndTime > dto.StartTime)
		{
			call.DurationSeconds = (int)(dto.EndTime.Value - dto.StartTime).TotalSeconds;
		}

		_context.Calls.Add(call);
		await _context.SaveChangesAsync();

		return await GetCallByIdAsync(call.Id) ?? throw new InvalidOperationException("Failed to create call");
	}

	public async Task<CallDto?> UpdateCallAsync(int id, UpdateCallDto dto)
	{
		var call = await _context.Calls.FindAsync(id);
		if (call == null)
			return null;

		var fromPhone = await _context.Phones.FindAsync(dto.FromPhoneId);
		var toPhone = await _context.Phones.FindAsync(dto.ToPhoneId);

		if (fromPhone == null || toPhone == null)
			throw new ArgumentException("One or both phones not found");

		if (dto.FromPhoneId == dto.ToPhoneId)
			throw new ArgumentException("Cannot call the same phone");

		call.FromPhoneId = dto.FromPhoneId;
		call.ToPhoneId = dto.ToPhoneId;
		call.StartTime = dto.StartTime;
		call.EndTime = dto.EndTime;
		call.DurationSeconds = dto.DurationSeconds;
		call.Status = dto.Status;
		call.Cost = dto.Cost;

		if (dto.EndTime.HasValue && dto.EndTime > dto.StartTime)
		{
			call.DurationSeconds = (int)(dto.EndTime.Value - dto.StartTime).TotalSeconds;
		}

		await _context.SaveChangesAsync();
		return await GetCallByIdAsync(id);
	}

	public async Task<bool> DeleteCallAsync(int id)
	{
		var call = await _context.Calls.FindAsync(id);
		if (call == null)
			return false;

		_context.Calls.Remove(call);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<CallStatisticsDto> GetCallStatisticsAsync(int? phoneId, int? contactId, DateTime startDate, DateTime endDate)
	{
		var query = _context.Calls
			.Include(c => c.FromPhone)
			.Include(c => c.ToPhone)
			.Where(c => c.StartTime >= startDate && c.StartTime <= endDate);

		if (phoneId.HasValue)
		{
			query = query.Where(c => c.FromPhoneId == phoneId || c.ToPhoneId == phoneId);
		}
		else if (contactId.HasValue)
		{
			query = query.Where(c => c.FromPhone.ContactId == contactId || c.ToPhone.ContactId == contactId);
		}

		var calls = await query.ToListAsync();

		var incomingCalls = phoneId.HasValue
			? calls.Count(c => c.ToPhoneId == phoneId)
			: contactId.HasValue
				? calls.Count(c => c.ToPhone.ContactId == contactId)
				: 0;

		var outgoingCalls = phoneId.HasValue
			? calls.Count(c => c.FromPhoneId == phoneId)
			: contactId.HasValue
				? calls.Count(c => c.FromPhone.ContactId == contactId)
				: calls.Count;

		return new CallStatisticsDto
		{
			StartDate = startDate,
			EndDate = endDate,
			TotalCalls = calls.Count,
			IncomingCalls = incomingCalls,
			OutgoingCalls = outgoingCalls,
			MissedCalls = calls.Count(c => c.Status == CallStatus.Missed),
			TotalDurationSeconds = calls.Sum(c => c.DurationSeconds),
			TotalCost = calls.Sum(c => c.Cost ?? 0),
			AverageDurationSeconds = calls.Count > 0 ? calls.Average(c => c.DurationSeconds) : 0
		};
	}

	public async Task<BillingDto> GetBillingReportAsync(int? phoneId, int? contactId, DateTime startDate, DateTime endDate)
	{
		var query = _context.Calls
			.Include(c => c.FromPhone)
				.ThenInclude(p => p.Contact)
			.Include(c => c.ToPhone)
				.ThenInclude(p => p.Contact)
			.Where(c => c.StartTime >= startDate && c.StartTime <= endDate);

		if (phoneId.HasValue)
		{
			query = query.Where(c => c.FromPhoneId == phoneId || c.ToPhoneId == phoneId);
		}
		else if (contactId.HasValue)
		{
			query = query.Where(c => c.FromPhone.ContactId == contactId || c.ToPhone.ContactId == contactId);
		}

		var calls = await query
			.OrderByDescending(c => c.StartTime)
			.ToListAsync();

		var billingItems = _mapper.Map<List<BillingItemDto>>(calls);

		return new BillingDto
		{
			StartDate = startDate,
			EndDate = endDate,
			Items = billingItems
		};
	}
}