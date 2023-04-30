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
            var result = await _authenticationService.Login(loginData);

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
        [Route("token")]
        [Authorize]
        public async Task<IActionResult> RefreshAccesToken([FromBody] RefreshTokenDto accessTokenRefreshDto)
        {
            var newAccessToken = _authenticationService.RenewAccesToken(accessTokenRefreshDto.RefreshToken);

            var accessTokenDto = new AccesTokenDto
            {
                AccessToken = newAccessToken
            };

            return new OkObjectResult(accessTokenDto);
        }

    }

}
