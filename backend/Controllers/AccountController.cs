using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentsApi.Services;
using StudentsApi.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticate _authenticate;

        public AccountController(IConfiguration configuration, IAuthenticate authenticate)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _authenticate = authenticate ?? throw new ArgumentNullException(nameof(authenticate));
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserToken>> CreateUserAsync([FromBody] RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                return BadRequest(ModelState);
            }
            var result = await _authenticate.RegisterUserAsync(model.Email, model.Password);

            if (result)
            {
                return Ok($"User created");
            }
            else
            {
                ModelState.AddModelError("CreateUserAsync", "Wrong Register");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> LoginUserAsync([FromBody] LoginModel userInfo)
        {
            var result = await _authenticate.AuthenticateUserAsync(userInfo.Email, userInfo.Password);
            if (result)
            {
                return GenerateToken(userInfo);
            }
            else
            {
                ModelState.AddModelError("LoginUserAsync", "Wrong Login");
                return BadRequest(ModelState);
            }
        }

        private ActionResult<UserToken> GenerateToken(LoginModel userInfo)
        {
            var claims = new[]
            {
                new Claim("email", userInfo.Email),
                new Claim("myToken", "application token"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(60);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
            };
        }
    }
}