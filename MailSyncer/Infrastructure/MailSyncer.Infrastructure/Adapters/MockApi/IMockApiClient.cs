using MailSyncer.Domain.Entities;
using MailSyncer.Infrastructure.HttpClients.Models;

namespace MailSyncer.Infrastructure.Adapters.MockApi
{
    public interface IMockApiClient
    {
        Task<ResponseWrapper<List<Contact>>> GetContactsAsync();
    }
}