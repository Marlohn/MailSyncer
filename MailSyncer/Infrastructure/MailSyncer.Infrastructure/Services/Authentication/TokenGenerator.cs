using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MailSyncer.Domain.Interfaces.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace MailSyncer.Infrastructure.Services.Authentication
{
    public class TokenGenerator : ITokenGenerator
    {
        private const string SecretKey = "df8532b4fc004a8fb72f6f71cae4303e";

        public string GenerateToken(string username, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: "MailSyncer",
                audience: "MailSyncerApi",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}