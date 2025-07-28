using System.ComponentModel.DataAnnotations;
using ContactCalls.Domain.Entities;

namespace ContactCalls.Dtos;

public class CreateCallDto
{
    [Required(ErrorMessage = "Исходящий номер обязателен")]
    public int FromPhoneId { get; set; }
    
    [Required(ErrorMessage = "Входящий номер обязателен")]
    public int ToPhoneId { get; set; }
    
    [Required(ErrorMessage = "Время начала обязательно")]
    public DateTime StartTime { get; set; }
    
    public DateTime? EndTime { get; set; }
    
    [Range(0, int.MaxValue, ErrorMessage = "Длительность не может быть отрицательной")]
    public int DurationSeconds { get; set; }
    
    public CallStatus Status { get; set; }
    
    [Range(0, 999999.99, ErrorMessage = "Стоимость должна быть от 0 до 999999.99")]
    public decimal? Cost { get; set; }
}
