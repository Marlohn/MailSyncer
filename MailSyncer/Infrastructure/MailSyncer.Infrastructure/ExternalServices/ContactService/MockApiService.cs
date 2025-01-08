using System.Text.Json;
using MailSyncer.Domain.Entities;
using MailSyncer.Domain.Interfaces;

namespace MailSyncer.Infrastructure.ExternalServices.ContactService
{
    public class MockApiService : IContactService
    {
        private readonly HttpClient _httpClient;

        public MockApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync()
        {
            var response = await _httpClient.GetAsync("contacts");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var contacts = JsonSerializer.Deserialize<IEnumerable<Contact>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return contacts;
        }
    }
}