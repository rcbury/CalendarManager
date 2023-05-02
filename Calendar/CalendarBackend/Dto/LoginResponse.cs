using Microsoft.AspNetCore.Identity;

namespace CalendarBackend.Dto;

public class LoginResponse
{
    public bool Result { get; set; }
    public IEnumerable<IdentityError>? Errors {get;set;}
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
