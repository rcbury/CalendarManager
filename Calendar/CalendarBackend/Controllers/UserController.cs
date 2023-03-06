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
            Console.WriteLine("poel");
            UserDto result = null;

            if (ModelState.IsValid)
            {
                result = await _authenticationService.RegisterUserAsync(registrationData);
            }


            if (result != null)
            {
                Console.WriteLine("userCreated");
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [HttpGet()]
        public IActionResult Test()
        {
            Console.WriteLine("poel");
            return new OkResult();
        }


    }
}
