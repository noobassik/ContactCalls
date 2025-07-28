namespace ContactCalls.Dtos;

public class BillingDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<BillingItemDto> Items { get; set; } = new();
    public decimal TotalCost => Items.Sum(i => i.Cost ?? 0);
    public int TotalDurationSeconds => Items.Sum(i => i.DurationSeconds);
    public string TotalDurationFormatted => FormatDuration(TotalDurationSeconds);
    
    private string FormatDuration(int seconds)
    {
        var time = TimeSpan.FromSeconds(seconds);
        return $"{(int)time.TotalHours:00}:{time.Minutes:00}:{time.Seconds:00}";
    }
}
