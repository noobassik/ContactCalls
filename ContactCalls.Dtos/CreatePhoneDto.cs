using System.ComponentModel.DataAnnotations;

namespace ContactCalls.Dtos;

public class CreatePhoneDto
{
    [Required(ErrorMessage = "ID контакта обязателен")]
    public int ContactId { get; set; }
    
    [Required(ErrorMessage = "Номер телефона обязателен")]
    [RegularExpression(@"^\+[0-9]-[0-9]{3}-[0-9]{3}-[0-9]{2}-[0-9]{2}$", 
        ErrorMessage = "Номер должен быть в формате +X-XXX-XXX-XX-XX")]
    public string Number { get; set; } = string.Empty;
    
    [StringLength(200, ErrorMessage = "Описание не должно превышать 200 символов")]
    public string? Description { get; set; }
    
    public bool IsPrimary { get; set; }
}
