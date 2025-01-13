namespace MailSyncer.Domain.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        string Authenticate(string username, string password, out string role);
    }
}