using ContactCalls.Dtos;

namespace ContactCalls.Services.Interfaces;

public interface IConferenceService
{
    Task<IEnumerable<ConferenceDto>> GetAllConferencesAsync();
    Task<ConferenceDto?> GetConferenceByIdAsync(int id);
    Task<ConferenceDto> CreateConferenceAsync(string name, DateTime startTime);
    Task<bool> AddParticipantAsync(int conferenceId, int phoneId);
    Task<bool> RemoveParticipantAsync(int conferenceId, int phoneId);
    Task<bool> StartConferenceAsync(int conferenceId);
    Task<bool> EndConferenceAsync(int conferenceId);
    Task<bool> DeleteConferenceAsync(int id);
}