using ContactCalls.Dtos;

namespace ContactCalls.Services.Interfaces;

public interface IPhoneService
{
    Task<IEnumerable<PhoneDto>> GetAllPhonesAsync();
    Task<PhoneDto?> GetPhoneByIdAsync(int id);
    Task<PhoneDto?> GetPhoneByNumberAsync(string number);
    Task<IEnumerable<PhoneDto>> GetPhonesByContactIdAsync(int contactId);
    Task<PhoneDto> CreatePhoneAsync(CreatePhoneDto dto);
    Task<PhoneDto?> UpdatePhoneAsync(int id, UpdatePhoneDto dto);
    Task<bool> DeletePhoneAsync(int id);
    Task<bool> SetPrimaryPhoneAsync(int phoneId);
}