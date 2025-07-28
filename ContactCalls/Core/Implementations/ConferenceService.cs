using AutoMapper;
using ContactCalls.Domain.Entities;
using ContactCalls.Dtos;
using ContactCalls.Infrastructure;
using ContactCalls.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactCalls.Services.Implementations;

public class ConferenceService : IConferenceService
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;

	public ConferenceService(ApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IEnumerable<ConferenceDto>> GetAllConferencesAsync()
	{
		var conferences = await _context.Conferences
			.Include(c => c.Participants)
				.ThenInclude(p => p.Phone)
					.ThenInclude(ph => ph.Contact)
			.OrderByDescending(c => c.StartTime)
			.ToListAsync();

		return _mapper.Map<IEnumerable<ConferenceDto>>(conferences);
	}

	public async Task<ConferenceDto?> GetConferenceByIdAsync(int id)
	{
		var conference = await _context.Conferences
			.Include(c => c.Participants)
				.ThenInclude(p => p.Phone)
					.ThenInclude(ph => ph.Contact)
			.FirstOrDefaultAsync(c => c.Id == id);

		return conference == null ? null : _mapper.Map<ConferenceDto>(conference);
	}

	public async Task<ConferenceDto> CreateConferenceAsync(string name, DateTime startTime)
	{
		var conference = new Conference
		{
			Name = name,
			StartTime = startTime,
			Status = ConferenceStatus.Scheduled
		};

		_context.Conferences.Add(conference);
		await _context.SaveChangesAsync();

		return await GetConferenceByIdAsync(conference.Id) ?? throw new InvalidOperationException("Failed to create conference");
	}

	public async Task<bool> AddParticipantAsync(int conferenceId, int phoneId)
	{
		var conference = await _context.Conferences.FindAsync(conferenceId);
		var phone = await _context.Phones.FindAsync(phoneId);

		if (conference == null || phone == null)
			return false;

		var existingParticipant = await _context.ConferenceParticipants
			.FirstOrDefaultAsync(cp => cp.ConferenceId == conferenceId && cp.PhoneId == phoneId);

		if (existingParticipant != null)
			return false;

		var participant = new ConferenceParticipant
		{
			ConferenceId = conferenceId,
			PhoneId = phoneId,
			JoinTime = DateTime.UtcNow
		};

		_context.ConferenceParticipants.Add(participant);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> RemoveParticipantAsync(int conferenceId, int phoneId)
	{
		var participant = await _context.ConferenceParticipants
			.FirstOrDefaultAsync(cp => cp.ConferenceId == conferenceId && cp.PhoneId == phoneId);

		if (participant == null)
			return false;

		if (participant.LeaveTime == null)
		{
			participant.LeaveTime = DateTime.UtcNow;
			participant.DurationSeconds = (int)(participant.LeaveTime.Value - participant.JoinTime).TotalSeconds;
		}
		else
		{
			_context.ConferenceParticipants.Remove(participant);
		}

		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> StartConferenceAsync(int conferenceId)
	{
		var conference = await _context.Conferences.FindAsync(conferenceId);
		if (conference == null || conference.Status != ConferenceStatus.Scheduled)
			return false;

		conference.Status = ConferenceStatus.InProgress;
		conference.StartTime = DateTime.UtcNow;

		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> EndConferenceAsync(int conferenceId)
	{
		var conference = await _context.Conferences
			.Include(c => c.Participants)
			.FirstOrDefaultAsync(c => c.Id == conferenceId);

		if (conference == null || conference.Status != ConferenceStatus.InProgress)
			return false;

		conference.Status = ConferenceStatus.Completed;
		conference.EndTime = DateTime.UtcNow;
		conference.DurationSeconds = (int)(conference.EndTime.Value - conference.StartTime).TotalSeconds;

		var activeParticipants = conference.Participants.Where(p => p.LeaveTime == null);
		foreach (var participant in activeParticipants)
		{
			participant.LeaveTime = conference.EndTime;
			participant.DurationSeconds = (int)(participant.LeaveTime.Value - participant.JoinTime).TotalSeconds;
		}

		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteConferenceAsync(int id)
	{
		var conference = await _context.Conferences.FindAsync(id);
		if (conference == null)
			return false;

		_context.Conferences.Remove(conference);
		await _context.SaveChangesAsync();
		return true;
	}
}