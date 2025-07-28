namespace ContactCalls.Dtos;

public class CallStatisticsDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalCalls { get; set; }
    public int IncomingCalls { get; set; }
    public int OutgoingCalls { get; set; }
    public int MissedCalls { get; set; }
    public int TotalDurationSeconds { get; set; }
    public string TotalDurationFormatted => FormatDuration(TotalDurationSeconds);
    public decimal TotalCost { get; set; }
    public double AverageDurationSeconds { get; set; }
    public string AverageDurationFormatted => FormatDuration((int)AverageDurationSeconds);
    
    private string FormatDuration(int seconds)
    {
        var time = TimeSpan.FromSeconds(seconds);
        return $"{(int)time.TotalHours:00}:{time.Minutes:00}:{time.Seconds:00}";
    }
}
