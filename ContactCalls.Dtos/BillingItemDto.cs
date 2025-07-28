namespace ContactCalls.Dtos;

public class BillingItemDto
{
    public int CallId { get; set; }
    public DateTime CallDate { get; set; }
    public string FromPhoneNumber { get; set; } = string.Empty;
    public string ToPhoneNumber { get; set; } = string.Empty;
    public int DurationSeconds { get; set; }
    public string DurationFormatted => FormatDuration(DurationSeconds);
    public decimal? Cost { get; set; }
    
    private string FormatDuration(int seconds)
    {
        var time = TimeSpan.FromSeconds(seconds);
        return $"{(int)time.TotalHours:00}:{time.Minutes:00}:{time.Seconds:00}";
    }
}
