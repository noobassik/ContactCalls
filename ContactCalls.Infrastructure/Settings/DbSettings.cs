namespace ContactCalls.Infrastructure.Settings;

public class DbSettings
{
    public const string Section = "DbSettings";
    public string ConnectionString { get; set; } = string.Empty;
}
