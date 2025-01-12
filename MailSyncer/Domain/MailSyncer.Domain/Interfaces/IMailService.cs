using MailSyncer.Domain.Entities;

namespace MailSyncer.Domain.Interfaces
{
    public interface IMailService
    {
        Task<SyncContactsResult> SyncContactsAsync(IEnumerable<Contact> contacts);
    }
}