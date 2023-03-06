using System.ComponentModel.DataAnnotations;

namespace CalendarBackend.Dto
{
    public class UserRegistrationDto
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
