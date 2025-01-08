namespace MailSyncer.Domain.Interfaces
{
    public interface IMailService
    {
        Task AddContactAsync(string email, string firstName, string lastName);
    }
}