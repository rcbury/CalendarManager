using Microsoft.AspNetCore.Identity;

namespace CalendarBackend.Dto
{
    public class RegistrationResponse
    {
        public UserDto? UserDto { get; set; }
        public bool Result { get; set; }
        public IEnumerable<IdentityError>? Errors { get; set; }
    }
}
