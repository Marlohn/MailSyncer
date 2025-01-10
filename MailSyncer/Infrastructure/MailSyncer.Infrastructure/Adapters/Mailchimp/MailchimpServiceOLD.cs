using System.Text;
using System.Text.Json;
using MailSyncer.Domain.Entities;
using MailSyncer.Domain.Interfaces;
using MailSyncer.Infrastructure.Adapters.Mailchimp.Models;

namespace MailSyncer.Infrastructure.Adapters.Mailchimp
{
    public class MailchimpServiceOLD : IMailService
    {
        private readonly HttpClient _httpClient;
        private readonly string _defaultListName = "MARLOHN CHOINSKI";

        public MailchimpServiceOLD(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddContactsAsync(IEnumerable<Contact> contacts)
        {
            string listId = await GetDefaultListId();

            foreach (var contact in contacts)
            {
                await AddContactToListAsync(listId, contact);
            }
        }

        private async Task<string> GetDefaultListId()
        {
            //Mailchimp's free plan allows only one list and does not permit the removal of the default list

            HttpResponseMessage response = await _httpClient.GetAsync("lists");

            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching lists: {response.StatusCode} - {response.ReasonPhrase}"); // maybe use jsonResponse
            }

            var listResponse = JsonSerializer.Deserialize<ListResponse>(jsonResponse);

            if (listResponse?.Lists != null && listResponse.Lists.Count != 0)
            {
                var defaultList = listResponse.Lists.SingleOrDefault(x => x.Name == _defaultListName);

                if (defaultList != null)
                {
                    return defaultList.Id;
                }
            }

            throw new Exception($"Default list not found.");
        }

        private async Task<string> Create()
        {
            var defaultList = new MailchimpList()
            {
                Id = string.Empty,
                Name = _defaultListName,
                Contact = new MailchimpContact()
                {
                    Company = "Marlohn Company",
                    Address1 = "123 Street",
                    City = "City",
                    State = "State",
                    Country = "BR",
                    Zip = "12345"
                },
                PermissionReminder = "You are receiving this email because you signed up for updates.",
                CampaignDefaults = new MailchimpCampaignDefaults()
                {
                    FromName = "Marlohn",
                    FromEmail = "marlohn@gmail.com",
                    Subject = "Welcome!",
                    Language = "en"
                }
            };

            string jsonContent = JsonSerializer.Serialize(defaultList);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("lists", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                string errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating list: {response.StatusCode} - {response.ReasonPhrase}");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var createdList = JsonSerializer.Deserialize<MailchimpList>(jsonResponse);

            if (createdList == null || string.IsNullOrEmpty(createdList.Id))
            {
                throw new Exception("Error creating the list: invalid API response.");
            }

            return createdList.Id;
        }

        private async Task AddContactToListAsync(string listId, Contact contact)
        {
            var member = new
            {
                email_address = contact.Email,
                status = "subscribed",
                merge_fields = new
                {
                    FNAME = contact.FirstName,
                    LNAME = contact.LastName
                }
            };

            string jsonContent = JsonSerializer.Serialize(member);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"lists/{listId}/members", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error adding contact {contact.Email}: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
    }
}