using MailSyncer.Domain.Entities;

namespace MailSyncer.Infrastructure.Adapters.MockApi
{
    public interface IMockApiClient
    {
        Task<List<Contact>> GetContactsAsync();
    }
}