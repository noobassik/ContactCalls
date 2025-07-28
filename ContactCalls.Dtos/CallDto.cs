using ContactCalls.Domain.Entities;

namespace ContactCalls.Dtos;

public class CallDto
{
    public int Id { get; set; }
    public int FromPhoneId { get; set; }
    public int ToPhoneId { get; set; }
    public string FromPhoneNumber { get; set; } = string.Empty;
    public string ToPhoneNumber { get; set; } = string.Empty;
    public string FromContactName { get; set; } = string.Empty;
    public string ToContactName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int DurationSeconds { get; set; }
    public string DurationFormatted => FormatDuration(DurationSeconds);
    public CallStatus Status { get; set; }
    public decimal? Cost { get; set; }
    
    private string FormatDuration(int seconds)
    {
        var time = TimeSpan.FromSeconds(seconds);
        return $"{(int)time.TotalHours:00}:{time.Minutes:00}:{time.Seconds:00}";
    }
}