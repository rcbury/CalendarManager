using System.ComponentModel.DataAnnotations;

namespace CalendarBackend.Dto
{
    public class UserDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? AvatarPath { get; set; }
    }
}
