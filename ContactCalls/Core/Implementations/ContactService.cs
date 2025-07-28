using AutoMapper;
using ContactCalls.Domain.Entities;
using ContactCalls.Dtos;
using ContactCalls.Infrastructure;
using ContactCalls.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactCalls.Services.Implementations;

public class ContactService : IContactService
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;

	public ContactService(ApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IEnumerable<ContactDto>> GetAllContactsAsync()
	{
		var contacts = await _context.Contacts
			.Include(c => c.Profile)
			.Include(c => c.Phones)
			.OrderBy(c => c.LastName)
			.ThenBy(c => c.FirstName)
			.ToListAsync();

		return _mapper.Map<IEnumerable<ContactDto>>(contacts);
	}

	public async Task<ContactDto?> GetContactByIdAsync(int id)
	{
		var contact = await _context.Contacts
			.Include(c => c.Profile)
			.Include(c => c.Phones)
			.FirstOrDefaultAsync(c => c.Id == id);

		return contact == null ? null : _mapper.Map<ContactDto>(contact);
	}

	public async Task<ContactDto> CreateContactAsync(CreateContactDto dto)
	{
		var contact = _mapper.Map<Contact>(dto);
		contact.CreatedAt = DateTime.UtcNow;

		_context.Contacts.Add(contact);
		await _context.SaveChangesAsync();

		return await GetContactByIdAsync(contact.Id) ?? throw new InvalidOperationException("Failed to create contact");
	}

	public async Task<ContactDto?> UpdateContactAsync(int id, UpdateContactDto dto)
	{
		var contact = await _context.Contacts.FindAsync(id);
		if (contact == null)
			return null;

		_mapper.Map(dto, contact);
		contact.UpdatedAt = DateTime.UtcNow;

		await _context.SaveChangesAsync();
		return await GetContactByIdAsync(id);
	}

	public async Task<bool> DeleteContactAsync(int id)
	{
		var contact = await _context.Contacts.FindAsync(id);
		if (contact == null)
			return false;

		_context.Contacts.Remove(contact);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<IEnumerable<ContactDto>> SearchContactsAsync(string searchTerm)
	{
		var contacts = await _context.Contacts
			.Include(c => c.Profile)
			.Include(c => c.Phones)
			.Where(c => c.FirstName.Contains(searchTerm) ||
					   c.LastName.Contains(searchTerm) ||
					   (c.MiddleName != null && c.MiddleName.Contains(searchTerm)) ||
					   (c.Profile != null && c.Profile.Email != null && c.Profile.Email.Contains(searchTerm)) ||
					   c.Phones.Any(p => p.Number.Contains(searchTerm)))
			.OrderBy(c => c.LastName)
			.ThenBy(c => c.FirstName)
			.ToListAsync();

		return _mapper.Map<IEnumerable<ContactDto>>(contacts);
	}

	public async Task<ContactProfileDto?> GetContactProfileAsync(int contactId)
	{
		var profile = await _context.ContactProfiles
			.FirstOrDefaultAsync(cp => cp.ContactId == contactId);

		return profile == null ? null : _mapper.Map<ContactProfileDto>(profile);
	}

	public async Task<ContactProfileDto> CreateContactProfileAsync(int contactId, CreateContactProfileDto dto)
	{
		var contact = await _context.Contacts.FindAsync(contactId);
		if (contact == null)
			throw new ArgumentException("Contact not found");

		var existingProfile = await _context.ContactProfiles
			.FirstOrDefaultAsync(cp => cp.ContactId == contactId);
		if (existingProfile != null)
			throw new InvalidOperationException("Contact profile already exists");

		var profile = _mapper.Map<ContactProfile>(dto);
		profile.ContactId = contactId;

		_context.ContactProfiles.Add(profile);
		await _context.SaveChangesAsync();

		return _mapper.Map<ContactProfileDto>(profile);
	}

	public async Task<ContactProfileDto?> UpdateContactProfileAsync(int contactId, CreateContactProfileDto dto)
	{
		var profile = await _context.ContactProfiles
			.FirstOrDefaultAsync(cp => cp.ContactId == contactId);

		if (profile == null)
			return null;

		_mapper.Map(dto, profile);
		await _context.SaveChangesAsync();

		return _mapper.Map<ContactProfileDto>(profile);
	}

	public async Task<bool> DeleteContactProfileAsync(int contactId)
	{
		var profile = await _context.ContactProfiles
			.FirstOrDefaultAsync(cp => cp.ContactId == contactId);

		if (profile == null)
			return false;

		_context.ContactProfiles.Remove(profile);
		await _context.SaveChangesAsync();
		return true;
	}
}