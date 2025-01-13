using MailSyncer.Application.Dtos;

namespace MailSyncer.Application.Interfaces
{
    public interface IAuthenticationServiceApplication
    {
        LoginResponseDto? AuthenticateUser(LoginRequestDto request);
    }
}