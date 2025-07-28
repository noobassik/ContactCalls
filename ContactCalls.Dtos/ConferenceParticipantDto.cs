namespace ContactCalls.Dtos;

public class ConferenceParticipantDto
{
    public int Id { get; set; }
    public int ConferenceId { get; set; }
    public int PhoneId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public DateTime JoinTime { get; set; }
    public DateTime? LeaveTime { get; set; }
    public int DurationSeconds { get; set; }
}