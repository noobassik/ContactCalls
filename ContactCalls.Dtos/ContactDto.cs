namespace ContactCalls.Dtos;

public class ContactDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ContactProfileDto? Profile { get; set; }
    public List<PhoneDto> Phones { get; set; } = new();
}