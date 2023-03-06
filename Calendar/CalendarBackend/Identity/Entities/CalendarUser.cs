using Microsoft.AspNetCore.Identity;

namespace CalendarBackend.Identity.Entities
{
    public class CalendarUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }

}
