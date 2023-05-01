using System.ComponentModel.DataAnnotations;

namespace CalendarBackend.Dto;

public class AvatarUpdateDto
{
	[Required]
	public IFormFile Avatar {get; set;} = null!;
}
