namespace ContactCalls.Domain.Entities;

public class Contact
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public virtual ContactProfile? Profile { get; set; }
    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
}