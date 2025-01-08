using System.Text;
using System.Text.Json;
using MailSyncer.Domain.Interfaces;

namespace MailSyncer.Infrastructure.ExternalServices.MailService
{
    public class MailchimpMailService : IMailService
    {
        private readonly HttpClient _httpClient;

        public MailchimpMailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddContactAsync(string email, string firstName, string lastName)
        {
            var requestBody = new
            {
                email_address = email,
                status = "subscribed",
                merge_fields = new
                {
                    FNAME = firstName,
                    LNAME = lastName
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://<mailchimp-api-endpoint>/lists", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to add contact to Mailchimp.");
            }
        }
    }
}