namespace ContactCalls.Dtos;

public class ContactProfileDto
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? Company { get; set; }
    public string? Position { get; set; }
    public string? Notes { get; set; }
}