using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CalendarBackend.Db;
using CalendarBackend.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CalendarBackend.Services
{
    public interface IAuthenticationService
    {
        Task<UserDto>? RegisterUserAsync(UserRegistrationDto userRegistrationData);
        Task<UserDto>? Login(LoginDto loginData);
    }

    public class JwtAuthenticationService : IAuthenticationService
    {
        private readonly UserManager<CalendarUser> _userManager;
        private readonly SignInManager<CalendarUser> _signinManager;
        private readonly IConfiguration _configuration;

        public JwtAuthenticationService(
            UserManager<CalendarUser> userManager,
            SignInManager<CalendarUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signinManager = signInManager;
            _configuration = configuration;
        }

        async public Task<UserDto>? RegisterUserAsync(UserRegistrationDto userRegistrationData)
        {
            var newUser = new CalendarUser
            {
                Email = userRegistrationData.Email,
                UserName = userRegistrationData.UserName,
            };


            var result = await _userManager.CreateAsync(newUser, userRegistrationData.Password);

            var userDto = new UserDto
            {
                Email = newUser.Email,
                UserName = newUser.UserName,
            };

            if (result.Succeeded)
                return userDto;
            else
                return null;

        }

        async public Task<UserDto> Login(LoginDto loginData)
        {
            var user = await _userManager.FindByEmailAsync(loginData.Email);

            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginData.Password);

            if (passwordCheck)
            {
                var claims = new[]
                {
                    new Claim("Email", user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));


                var token = new JwtSecurityToken(
                    issuer: _configuration["AuthSettings:Issuer"],
                    audience: _configuration["AuthSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
					signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

				string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            }



        }
    }
}
