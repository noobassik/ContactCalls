using System.ComponentModel.DataAnnotations;

namespace ContactCalls.Dtos;

public class CreateContactProfileDto
{
    [EmailAddress(ErrorMessage = "Некорректный email")]
    public string? Email { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    [StringLength(500, ErrorMessage = "Адрес не должен превышать 500 символов")]
    public string? Address { get; set; }
    
    [StringLength(200, ErrorMessage = "Компания не должна превышать 200 символов")]
    public string? Company { get; set; }
    
    [StringLength(200, ErrorMessage = "Должность не должна превышать 200 символов")]
    public string? Position { get; set; }
    
    [StringLength(2000, ErrorMessage = "Заметки не должны превышать 2000 символов")]
    public string? Notes { get; set; }
}
