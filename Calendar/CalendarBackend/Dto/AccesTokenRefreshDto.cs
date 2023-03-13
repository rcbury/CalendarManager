using System.ComponentModel.DataAnnotations;

namespace CalendarBackend.Dto;

public class RefreshTokenDto
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}
