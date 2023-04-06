
using CalendarBackend.Dto;
using CalendarBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalendarBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthorizationController(
            IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost(Name = "login user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        public async Task<IActionResult> Login([FromBody] LoginDto loginData)
        {
            LoginResponse result = null;

            if (ModelState.IsValid)
            {
                result = await _authenticationService.Login(loginData);
            }

            if (result != null)
            {
                Console.WriteLine("logged in");
                Console.WriteLine(result.AccessToken);
                return Ok(result);
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [HttpGet()]
        [Authorize]
        public IActionResult Test()
        {
            Console.WriteLine("poel");
            return new OkResult();
        }

        [HttpPost]
        [Route("token")]
        [Authorize]
        public async Task<IActionResult> RefreshAccesToken([FromBody] RefreshTokenDto accessTokenRefreshDto)
        {
            var newAccessToken = _authenticationService.RenewAccesToken(accessTokenRefreshDto.RefreshToken);

            var accessTokenDto = new AccesTokenDto
            {
                AccessToken = newAccessToken
            };

            return Ok(accessTokenDto);
        }

    }

}
