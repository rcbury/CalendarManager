using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CalendarBackend.Db;
using CalendarBackend.Dto;
using CalendarBackend.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CalendarBackend.Services
{
    public interface IAuthenticationService
    {
        Task<UserDto>? RegisterUserAsync(UserRegistrationDto userRegistrationData);
        Task<LoginResponse>? Login(LoginDto loginData);
		string RenewAccesToken(string refreshToken);
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

        async public Task<LoginResponse>? Login(LoginDto loginData)
        {
            var user = await _userManager.FindByEmailAsync(loginData.Email);

            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginData.Password);

            if (!passwordCheck)
            {
                return null;
            }

            var accessToken = GenerateJwtAccesToken(user.Email, user.Id.ToString());
            var refreshToken = GenerateJwtRefreshToken(user.Email, user.Id.ToString());

            var loginResponse = new LoginResponse
            {
				RefreshToken = refreshToken,
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
                new Claim("typ", JwtTokenTypes.Access)
            };

            //TODO: Place token settings into config file

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("verylongsecretkey"));

            var token = new JwtSecurityToken(
                issuer: "issuer",
                audience: JwtTokenTypes.Access,
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
                new Claim("typ", JwtTokenTypes.Refresh)
            };

            //TODO: Place token settings into config file

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("verylongsecretkey"));

            var token = new JwtSecurityToken(
                issuer: "issuer",
                audience: JwtTokenTypes.Refresh,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenAsString;
        }

        public string RenewAccesToken(string refreshToken)
        {
			var jwtToken = ValidateRefreshToken(refreshToken);

            if (jwtToken == null)
                throw new Exception("Invalid token");

            var userId = jwtToken.Claims.First(x => x.Type == "userId");
            var email = jwtToken.Claims.First(x => x.Type == "email");

            var newAccesToken = GenerateJwtAccesToken(email.Value, userId.Value);

            return newAccesToken;
        }

        private JwtSecurityToken? ValidateRefreshToken(string refreshToken)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = JwtTokenTypes.Refresh,
                ValidIssuer = "issuer",
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("verylongsecretkey")),
                ValidateIssuerSigningKey = true,
            };

            SecurityToken validatedToken = null;

            var claimsPrincipal = jwtHandler.ValidateToken(refreshToken, tokenValidationParameters, out validatedToken);

            var validatedJwtToken = validatedToken as JwtSecurityToken;

			return validatedJwtToken;
        }
    }
}
