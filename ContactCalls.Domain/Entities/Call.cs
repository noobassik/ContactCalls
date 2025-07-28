namespace ContactCalls.Domain.Entities;

public class Call
{
    public int Id { get; set; }
    public int FromPhoneId { get; set; }
    public int ToPhoneId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int DurationSeconds { get; set; }
    public CallStatus Status { get; set; }
    public decimal? Cost { get; set; }
    
    public virtual Phone FromPhone { get; set; } = null!;
    public virtual Phone ToPhone { get; set; } = null!;
}