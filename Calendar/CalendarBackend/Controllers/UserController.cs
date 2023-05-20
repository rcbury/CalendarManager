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
        private readonly UserService _userService;
        private readonly UserManager<CalendarUser> _userManager;
        private readonly StaticFilesLinkCreator _staticFilesLinkCreator;

        public UserController(
            IAuthenticationService authenticationService,
            UserManager<CalendarUser> userManager,
            UserService userService,
            StaticFilesLinkCreator staticFilesLinkCreator)
        {
            _authenticationService = authenticationService;
            _userManager = userManager;
            _userService = userService;
            _staticFilesLinkCreator = staticFilesLinkCreator;
        }

        [HttpPost(Name = "Register a new user")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationData)
        {
            var result = await _authenticationService.RegisterUserAsync(registrationData);

            if (result.Result)
            {
                return new OkObjectResult(result);
            }
            else
            {
                return new BadRequestObjectResult(result);
            }
        }

        [HttpPost]
        [Route("/User/self/reset")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var charsBig = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                var charsSmall = "abcdefghijklmnopqrstuvwxyz";
                var numbers = "0123456789";
                var symbols = "!@#$%^&*";
                var stringChars = new char[12];
                var random = new Random();

                var newPassword = "";

                for (int i = 0; i < 3; i++)
                {
                    var count = random.Next(0, charsBig.Length);
                    newPassword += charsBig[count];
                }

                for (int i = 0; i < 3; i++)
                {
                    var count = random.Next(0, charsSmall.Length);
                    newPassword += charsSmall[count];
                }

                for (int i = 0; i < 3; i++)
                {
                    var count = random.Next(0, numbers.Length);
                    newPassword += numbers[count];
                }

                for (int i = 0; i < 3; i++)
                {
                    var count = random.Next(0, symbols.Length);
                    newPassword += symbols[count];
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var res = await _userManager.ResetPasswordAsync(user, token, newPassword);

                return new OkObjectResult(new PasswordChangeResponse
                {
                    NewPassword = newPassword
                });
            }

            return BadRequest();

        }

        [HttpPut]
        [Route("/User/self")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserDto userData)
        {
            var authorizedUserClaim = this.User;

            var user = await _userService.GetUserByClaim(authorizedUserClaim);

            var result = await _userService.UpdateUser(userData, user.Id);

            if (result.Result)
                return new OkObjectResult(result);
            else
                return new BadRequestObjectResult(result);
        }

        [HttpPut("/User/self/avatar")]
        [Authorize]
        public async Task<IActionResult> UpdateAvatar([FromForm] IFormFile profilePicture)
        {
            var authorizedUser = this.User;

            var userIdClaim = authorizedUser.Claims.Where(x => x.Type == "userId").FirstOrDefault();

            if (userIdClaim == null)
                return new BadRequestResult();

            var result = await _userService.UpdateAvatar(int.Parse(userIdClaim.Value), profilePicture);

            if (result)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }

        }

        [HttpPut("/User/self/password")]
        [Authorize]
        public async Task<IActionResult> ChangePassowrd([FromBody] ChangePasswordDto changePasswordDto)
        {
            var authorizedUser = this.User;

            var user = await _userService.GetUserByClaim(authorizedUser);

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (result.Succeeded)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestObjectResult(result.Errors);
            }

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
            var authorizedUserClaim = this.User;

            var user = await _userService.GetUserByClaim(authorizedUserClaim);

            if (user == null)
                return new BadRequestResult();

            var userResponseDTO = new UserDto();

            userResponseDTO.Id = user.Id;
            userResponseDTO.LastName = user.LastName;
            userResponseDTO.FirstName = user.FirstName;
            userResponseDTO.UserName = user.UserName;
            userResponseDTO.Email = user.Email;

            var result = _staticFilesLinkCreator.GetAvatarLink(int.Parse(user.Id.ToString()));

            userResponseDTO.AvatarPath = result;

            return new OkObjectResult(userResponseDTO);
        }
    }
}
