namespace ContactCalls.Domain.Entities;

public class Phone
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public string Number { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPrimary { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public virtual Contact Contact { get; set; } = null!;
    public virtual ICollection<Call> OutgoingCalls { get; set; } = new List<Call>();
    public virtual ICollection<Call> IncomingCalls { get; set; } = new List<Call>();
    public virtual ICollection<ConferenceParticipant> ConferenceParticipations { get; set; } = new List<ConferenceParticipant>();
}
