using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace VertexTestApi.Services
{
    public class AuthService(IConfiguration config)
    {
        private readonly string _tokenKey = config["Jwt_Key"]
            ?? throw new Exception("JWT key is missing from configuration.");

        public Task<string> CreateToken(IdentityUser user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
                throw new Exception("User does not have a valid username.");

            if (_tokenKey.Length < 64)
                throw new Exception("JWT key must be at least 64 characters for security purposes.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserName),
                new(ClaimTypes.Name, user.UserName),
                new Claim("username", user.UserName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}
