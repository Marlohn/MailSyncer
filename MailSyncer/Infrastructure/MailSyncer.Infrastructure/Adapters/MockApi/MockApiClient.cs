
using MailSyncer.Domain.Entities;

namespace MailSyncer.Infrastructure.Adapters.MockApi
{
    public class MockApiClient : BaseHttpClient, IMockApiClient
    {
        public MockApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            return await GetAsync<List<Contact>>("contacts");
        }
    }
}