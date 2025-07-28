using ContactCalls.Domain.Entities;

namespace ContactCalls.Dtos;

public class ConferenceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int DurationSeconds { get; set; }
    public ConferenceStatus Status { get; set; }
    public List<ConferenceParticipantDto> Participants { get; set; } = new();
    public int ParticipantCount => Participants.Count;
}