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
            var response = await _mockApiClient.GetContactsAsync();

            if (response.IsSuccessfulWithData)
                return response.Data;

            throw new InvalidOperationException(response.ErrorMessage);
        }
    }
}