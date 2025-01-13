using MailSyncer.Application.Dtos;
using MailSyncer.Application.Interfaces;
using MailSyncer.Domain.Interfaces.Authentication;

namespace MailSyncer.Application.Services
{
    public class AuthenticationServiceApplication : IAuthenticationServiceApplication
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationServiceApplication(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public LoginResponseDto? AuthenticateUser(LoginRequestDto request)
        {
            var token = _authenticationService.Authenticate(request.Username, request.Password, out string role);

            if (string.IsNullOrEmpty(token))
                return null;

            return new LoginResponseDto
            {
                Token = token,
                Role = role
            };
        }
    }
}