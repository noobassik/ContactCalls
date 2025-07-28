using ContactCalls.Dtos;

namespace ContactCalls.Services.Interfaces;

public interface IContactService
{
    Task<IEnumerable<ContactDto>> GetAllContactsAsync();
    Task<ContactDto?> GetContactByIdAsync(int id);
    Task<ContactDto> CreateContactAsync(CreateContactDto dto);
    Task<ContactDto?> UpdateContactAsync(int id, UpdateContactDto dto);
    Task<bool> DeleteContactAsync(int id);
    Task<IEnumerable<ContactDto>> SearchContactsAsync(string searchTerm);
    Task<ContactProfileDto?> GetContactProfileAsync(int contactId);
    Task<ContactProfileDto> CreateContactProfileAsync(int contactId, CreateContactProfileDto dto);
    Task<ContactProfileDto?> UpdateContactProfileAsync(int contactId, CreateContactProfileDto dto);
    Task<bool> DeleteContactProfileAsync(int contactId);
}