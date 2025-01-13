namespace MailSyncer.Domain.Interfaces.Authentication
{
    public interface ITokenGenerator
    {
        string GenerateToken(string username, string role);
    }
}