using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using VertexTestApi.Services;
using VertexTestApi.DTOs;

namespace VertexTestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _config;

        public AuthController(AuthService authService, IConfiguration config)
        {
            _authService = authService;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var configuredUsername = _config["Credentials_Username"];
            var configuredPassword = _config["Credentials_Password"];

            if (string.IsNullOrWhiteSpace(configuredUsername) || string.IsNullOrWhiteSpace(configuredPassword))
                return StatusCode(500, "Server credentials are not configured.");

            if (request.Username == configuredUsername && request.Password == configuredPassword)
            {
                var user = new IdentityUser { UserName = request.Username };

                var token = await _authService.CreateToken(user);

                return Ok(new LoginResponse { Token = token });
            }

            return Unauthorized("Invalid username or password.");
        }
    }
}
