using System.ComponentModel.DataAnnotations;

namespace CalendarBackend.Dto;

public class AccesTokenDto
{
    [Required]
    public string AccessToken { get; set; } = null!;
}
