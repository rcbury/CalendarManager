using CalendarBackend.Dto;
using CalendarBackend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalendarBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public UserController(
            IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost(Name = "Register a new user")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationData)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.RegisterUserAsync(registrationData);
            }


            if (true)
            {
                Console.WriteLine("userCreated");
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
