namespace ContactCalls.Domain.Entities;
public class Conference
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int DurationSeconds { get; set; }
    public ConferenceStatus Status { get; set; }
    
    public virtual ICollection<ConferenceParticipant> Participants { get; set; } = new List<ConferenceParticipant>();
}