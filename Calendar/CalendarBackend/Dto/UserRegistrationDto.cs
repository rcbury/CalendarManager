using System.ComponentModel.DataAnnotations;

namespace CalendarBackend.Dto
{
    public class UserRegistrationDto
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public string Password { get; set; } = null!;
    }
}
