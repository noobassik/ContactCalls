namespace ContactCalls.Dtos;

public class PhoneDto
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public string Number { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPrimary { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ContactName { get; set; } = string.Empty;
}