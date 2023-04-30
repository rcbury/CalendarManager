using CalendarBackend.Db;
using CalendarBackend.Dto;
using CalendarBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalendarBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly UserManager<CalendarUser> _userManager;

        public UserController(
            IAuthenticationService authenticationService,
            UserManager<CalendarUser> userManager)
        {
            _authenticationService = authenticationService;
            _userManager = userManager;
        }

        [HttpPost(Name = "Register a new user")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationData)
        {
            RegistrationResponse result = null;

			result = await _authenticationService.RegisterUserAsync(registrationData);


            if (result.Result)
            {
                return new OkObjectResult(result);
            }
            else
            {
                return new BadRequestObjectResult(result);
            }
        }

        [HttpPut(Name = "Change profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDto userData)
        {
            var authorizedUser = this.User;

            var userId = authorizedUser.Claims.Where(x => x.Type == "userId").FirstOrDefault();

            if (userId == null)
                return new BadRequestResult();

            var user = await _userManager.FindByIdAsync(userId.Value);

            if (user == null)
                throw new Exception("User not found");

            user.FirstName = userData.FirstName;
            user.LastName = userData.LastName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result.Errors);
            }

            var UserResponseDTO = new UserDto();
            UserResponseDTO.LastName = user.LastName;
            UserResponseDTO.FirstName = user.FirstName;
            UserResponseDTO.UserName = user.UserName;
            UserResponseDTO.Email = user.Email;

            return new OkObjectResult(user);
        }

        [HttpGet]
        [Route("/User/{userId}")]
        public async Task<IActionResult> Get([FromRoute] int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return new BadRequestResult();


            var UserResponseDTO = new UserDto();
            UserResponseDTO.LastName = user.LastName;
            UserResponseDTO.FirstName = user.FirstName;
            UserResponseDTO.UserName = user.UserName;
            UserResponseDTO.Email = user.Email;

            return new OkObjectResult(UserResponseDTO);
        }

        [HttpGet]
        [Route("/User/self")]
		[Authorize]
        public async Task<IActionResult> GetSelf()
        {
            var authorizedUser = this.User;

            var userId = authorizedUser.Claims.Where(x => x.Type == "userId").FirstOrDefault();

            if (userId == null)
                return new BadRequestResult();

            var user = await _userManager.FindByIdAsync(userId.Value);

            if (user == null)
                return new BadRequestResult();

            var UserResponseDTO = new UserDto();
            UserResponseDTO.LastName = user.LastName;
            UserResponseDTO.FirstName = user.FirstName;
            UserResponseDTO.UserName = user.UserName;
            UserResponseDTO.Email = user.Email;

            return new OkObjectResult(UserResponseDTO);
        }
    }
}
