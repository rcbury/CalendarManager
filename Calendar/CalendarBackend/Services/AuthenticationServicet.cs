using CalendarBackend.Dto;
using CalendarBackend.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace CalendarBackend.Services
{
    public interface IAuthenticationService
    {
        Task<UserDto> RegisterUserAsync(UserRegistrationDto userRegistrationData);
    }

    public class JwtAuthenticationService : IAuthenticationService
    {
        private readonly UserManager<CalendarUser> _userManager;
        private readonly SignInManager<CalendarUser> _signinManager;

        public JwtAuthenticationService(
            UserManager<CalendarUser> userManager,
            SignInManager<CalendarUser> signInManager)
        {
            _userManager = userManager;
            _signinManager = signInManager;
        }

        async public Task<UserDto> RegisterUserAsync(UserRegistrationDto userRegistrationData)
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

			return userDto;
        }
    }
}
