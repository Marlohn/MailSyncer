using MailSyncer.Domain.Entities;

namespace MailSyncer.Domain.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetContactsAsync();
    }
}