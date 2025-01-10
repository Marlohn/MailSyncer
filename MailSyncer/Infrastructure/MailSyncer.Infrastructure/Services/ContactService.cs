using MailSyncer.Domain.Entities;
using MailSyncer.Domain.Interfaces;
using MailSyncer.Infrastructure.Adapters.MockApi;

namespace MailSyncer.Infrastructure.Services
{
    public class ContactService : IContactService
    {
        private readonly IMockApiClient _mockApiClient;

        public ContactService(IMockApiClient mockApiClient)
        {
            _mockApiClient = mockApiClient;
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync()
        {
            return await _mockApiClient.GetContactsAsync();
        }
    }
}