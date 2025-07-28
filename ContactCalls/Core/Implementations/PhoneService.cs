using AutoMapper;
using ContactCalls.Domain.Entities;
using ContactCalls.Dtos;
using ContactCalls.Infrastructure;
using ContactCalls.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactCalls.Services.Implementations;

public class PhoneService : IPhoneService
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;

	public PhoneService(ApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IEnumerable<PhoneDto>> GetAllPhonesAsync()
	{
		var phones = await _context.Phones
			.Include(p => p.Contact)
			.OrderBy(p => p.Contact.LastName)
			.ThenBy(p => p.Contact.FirstName)
			.ThenBy(p => p.Number)
			.ToListAsync();

		return _mapper.Map<IEnumerable<PhoneDto>>(phones);
	}

	public async Task<PhoneDto?> GetPhoneByIdAsync(int id)
	{
		var phone = await _context.Phones
			.Include(p => p.Contact)
			.FirstOrDefaultAsync(p => p.Id == id);

		return phone == null ? null : _mapper.Map<PhoneDto>(phone);
	}

	public async Task<PhoneDto?> GetPhoneByNumberAsync(string number)
	{
		var phone = await _context.Phones
			.Include(p => p.Contact)
			.FirstOrDefaultAsync(p => p.Number == number);

		return phone == null ? null : _mapper.Map<PhoneDto>(phone);
	}

	public async Task<IEnumerable<PhoneDto>> GetPhonesByContactIdAsync(int contactId)
	{
		var phones = await _context.Phones
			.Include(p => p.Contact)
			.Where(p => p.ContactId == contactId)
			.OrderByDescending(p => p.IsPrimary)
			.ThenBy(p => p.Number)
			.ToListAsync();

		return _mapper.Map<IEnumerable<PhoneDto>>(phones);
	}

	public async Task<PhoneDto> CreatePhoneAsync(CreatePhoneDto dto)
	{
		var contact = await _context.Contacts.FindAsync(dto.ContactId);
		if (contact == null)
			throw new ArgumentException("Contact not found");

		var existingPhone = await _context.Phones
			.FirstOrDefaultAsync(p => p.Number == dto.Number);
		if (existingPhone != null)
			throw new InvalidOperationException("Phone number already exists");

		var phone = _mapper.Map<Phone>(dto);
		phone.CreatedAt = DateTime.UtcNow;

		var contactPhonesCount = await _context.Phones
			.CountAsync(p => p.ContactId == dto.ContactId);

		if (contactPhonesCount == 0 || dto.IsPrimary)
		{
			if (dto.IsPrimary)
			{
				var otherPhones = await _context.Phones
					.Where(p => p.ContactId == dto.ContactId && p.IsPrimary)
					.ToListAsync();

				foreach (var otherPhone in otherPhones)
				{
					otherPhone.IsPrimary = false;
				}
			}

			phone.IsPrimary = true;
		}

		_context.Phones.Add(phone);
		await _context.SaveChangesAsync();

		return await GetPhoneByIdAsync(phone.Id) ?? throw new InvalidOperationException("Failed to create phone");
	}

	public async Task<PhoneDto?> UpdatePhoneAsync(int id, UpdatePhoneDto dto)
	{
		var phone = await _context.Phones.FindAsync(id);
		if (phone == null)
			return null;

		if (phone.Number != dto.Number)
		{
			var existingPhone = await _context.Phones
				.FirstOrDefaultAsync(p => p.Number == dto.Number && p.Id != id);
			if (existingPhone != null)
				throw new InvalidOperationException("Phone number already exists");
		}

		if (dto.IsPrimary && !phone.IsPrimary)
		{
			var otherPhones = await _context.Phones
				.Where(p => p.ContactId == phone.ContactId && p.IsPrimary)
				.ToListAsync();

			foreach (var otherPhone in otherPhones)
			{
				otherPhone.IsPrimary = false;
			}
		}

		_mapper.Map(dto, phone);
		await _context.SaveChangesAsync();

		return await GetPhoneByIdAsync(id);
	}

	public async Task<bool> DeletePhoneAsync(int id)
	{
		var phone = await _context.Phones.FindAsync(id);
		if (phone == null)
			return false;

		var hasRelatedCalls = await _context.Calls
			.AnyAsync(c => c.FromPhoneId == id || c.ToPhoneId == id);

		if (hasRelatedCalls)
			throw new InvalidOperationException("Cannot delete phone with related calls");

		if (phone.IsPrimary)
		{
			var otherPhone = await _context.Phones
				.Where(p => p.ContactId == phone.ContactId && p.Id != id)
				.FirstOrDefaultAsync();

			if (otherPhone != null)
			{
				otherPhone.IsPrimary = true;
			}
		}

		_context.Phones.Remove(phone);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> SetPrimaryPhoneAsync(int phoneId)
	{
		var phone = await _context.Phones.FindAsync(phoneId);
		if (phone == null)
			return false;

		var otherPhones = await _context.Phones
			.Where(p => p.ContactId == phone.ContactId && p.Id != phoneId && p.IsPrimary)
			.ToListAsync();

		foreach (var otherPhone in otherPhones)
		{
			otherPhone.IsPrimary = false;
		}

		phone.IsPrimary = true;
		await _context.SaveChangesAsync();
		return true;
	}
}