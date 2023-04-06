using System.ComponentModel.DataAnnotations;

namespace CalendarBackend.Dto
{
    public class UserDto
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
