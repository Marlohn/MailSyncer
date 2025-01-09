using MailSyncer.Domain.Entities;

namespace MailSyncer.Domain.Interfaces
{
    public interface IMailService
    {
        Task AddContactsAsync(IEnumerable<Contact> contacts);
    }
}