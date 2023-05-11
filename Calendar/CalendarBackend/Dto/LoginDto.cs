using System.ComponentModel.DataAnnotations;

namespace CalendarBackend.Dto;

public class LoginDto
{
	[Required]
	[EmailAddress]
	[StringLength(50)]
	public string Email {get; set;} = null!;

	[Required]
	[StringLength(50, MinimumLength = 5)]
    public string Password { get; set; } = null!;
}
