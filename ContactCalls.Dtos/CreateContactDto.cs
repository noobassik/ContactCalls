using System.ComponentModel.DataAnnotations;

namespace ContactCalls.Dtos;

public class CreateContactDto
{
    [Required(ErrorMessage = "Имя обязательно")]
    [StringLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Фамилия обязательна")]
    [StringLength(100, ErrorMessage = "Фамилия не должна превышать 100 символов")]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(100, ErrorMessage = "Отчество не должно превышать 100 символов")]
    public string? MiddleName { get; set; }
}