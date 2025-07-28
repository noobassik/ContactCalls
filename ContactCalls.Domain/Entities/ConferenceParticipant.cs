namespace ContactCalls.Domain.Entities;

public class ConferenceParticipant
{
    public int Id { get; set; }
    public int ConferenceId { get; set; }
    public int PhoneId { get; set; }
    public DateTime JoinTime { get; set; }
    public DateTime? LeaveTime { get; set; }
    public int DurationSeconds { get; set; }
    
    public virtual Conference Conference { get; set; } = null!;
    public virtual Phone Phone { get; set; } = null!;
}