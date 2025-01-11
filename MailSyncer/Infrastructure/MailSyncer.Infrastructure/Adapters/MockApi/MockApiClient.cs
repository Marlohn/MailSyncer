using MailSyncer.Domain.Entities;
using MailSyncer.Infrastructure.HttpClients;
using MailSyncer.Infrastructure.HttpClients.Models;

namespace MailSyncer.Infrastructure.Adapters.MockApi
{
    public class MockApiClient : HttpClientBase, IMockApiClient
    {
        public MockApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ResponseWrapper<List<Contact>>> GetContactsAsync()
        {
            return await GetAsync<List<Contact>>("contacts");
        }
    }
}