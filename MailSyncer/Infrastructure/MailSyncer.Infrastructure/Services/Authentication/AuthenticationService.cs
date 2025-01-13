using MailSyncer.Domain.Interfaces.Authentication;

namespace MailSyncer.Infrastructure.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenGenerator _tokenGenerator;

        public AuthenticationService(ITokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        public string Authenticate(string username, string password, out string role)
        {
            // Simulate credencial validation:
            if (username == "admin" && password == "password")
            {
                role = "Admin";
                return _tokenGenerator.GenerateToken(username, role);
            }

            if (username == "user" && password == "password")
            {
                role = "User";
                return _tokenGenerator.GenerateToken(username, role);
            }

            role = string.Empty;

            return string.Empty;
        }
    }
}