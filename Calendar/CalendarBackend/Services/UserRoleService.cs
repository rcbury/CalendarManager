using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CalendarBackend.Db;
using CalendarBackend.Dto;
using CalendarBackend.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CalendarBackend.Services;

/* public interface IAuthenticationService */
/* { */
/*     Task<UserDto>? RegisterUserAsync(UserRegistrationDto userRegistrationData); */
/*     Task<LoginResponse>? Login(LoginDto loginData); */
/* } */

public class UserRoleService
{
    private readonly UserManager<CalendarUser> _userManager;
    private readonly SignInManager<CalendarUser> _signinManager;
    private readonly IConfiguration _configuration;
    private readonly CalendarDevContext _calendarDevContext;

    public UserRoleService(
        UserManager<CalendarUser> userManager,
        SignInManager<CalendarUser> signInManager,
        IConfiguration configuration,
        CalendarDevContext calendarDevContext)
    {
        _userManager = userManager;
        _signinManager = signInManager;
        _configuration = configuration;
        _calendarDevContext = calendarDevContext;
    }

    async public Task<UserRole>? GetUserRoomRole(int userId, int roomId)
    {
        var userRoom = _calendarDevContext.RoomUsers
            .Where(x => x.User.Id == userId)
            .Where(x => x.RoomId == roomId)
            .FirstOrDefault();

        if (userRoom == null)
            return null;

        var userRole = GetUserRole(userRoom.UserRoleId);


        return userRole;
    }

    public UserRole? GetUserRole(int userRoleId)
    {
        var userRole = _calendarDevContext.UserRoles
            .Where(x => x.Id == userRoleId)
            .FirstOrDefault();

        return userRole;
    }

    async public Task<LoginResponse>? Login(LoginDto loginData)
    {
        var user = await _userManager.FindByEmailAsync(loginData.Email);

        var passwordCheck = await _userManager.CheckPasswordAsync(user, loginData.Password);

        Console.WriteLine(passwordCheck);

        if (!passwordCheck)
        {
            return null;
        }

        var accessToken = GenerateJwtAccesToken(user.Email, user.Id.ToString());

        var loginResponse = new LoginResponse
        {
            AccessToken = accessToken
        };

        return loginResponse;
    }

    private string GenerateJwtAccesToken(string userEmail, string userId)
    {
        var claims = new[]
        {
                new Claim("email", userEmail),
                new Claim("userId", userId),
                new Claim("type", JwtTokenTypes.Access)
            };

        //TODO: Place token settings into config file

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("verylongsecretkey"));

        var token = new JwtSecurityToken(
            issuer: "issuer",
            audience: "audience",
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenAsString;
    }

    private string GenerateJwtRefreshToken(string userEmail, string userId)
    {
        var claims = new[]
        {
                new Claim("email", userEmail),
                new Claim("userId", userId),
                new Claim("type", JwtTokenTypes.Refresh)

            };

        //TODO: Place token settings into config file

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("verylongsecretkey"));

        var token = new JwtSecurityToken(
            issuer: "issuer",
            audience: "audience",
            claims: claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenAsString;
    }

    private string RenewAccesToken(string refreshToken)
    {
        var handler = new JwtSecurityTokenHandler();

        var jwtToken = handler.ReadToken(refreshToken) as JwtSecurityToken;

        if (jwtToken == null)
            throw new Exception("Invalid token");

        var tokenType = jwtToken.Claims.Where(x => x.Type == "type").FirstOrDefault();

        if (tokenType == null)
            throw new Exception("Invalid token");

        if (tokenType.Value != JwtTokenTypes.Refresh)
            throw new Exception("Invalid token");

        var expirationDateSeconds = long.Parse(jwtToken.Claims.First(x => x.Type == "exp").Value);

        var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expirationDateSeconds);

        if (expirationDate < DateTime.Now)
        {
            throw new Exception("refreshTokenExpired");
        }

        var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
        var email = jwtToken.Claims.First(x => x.Type == "Email");

        var newAccesToken = GenerateJwtAccesToken(email.Value, userId.Value);

        return newAccesToken;
    }

    private string ValidateRefreshToken(string refreshToken)
    {
        return "fasdfasd";
    }
}
