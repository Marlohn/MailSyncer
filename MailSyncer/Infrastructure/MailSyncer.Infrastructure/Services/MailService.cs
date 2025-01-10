using MailSyncer.Domain.Entities;
using MailSyncer.Domain.Interfaces;
using MailSyncer.Infrastructure.Adapters.Mailchimp;
using MailSyncer.Infrastructure.Adapters.Mailchimp.Models;

namespace MailSyncer.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly IMailchimpClient _mailchimpClient;
        private readonly string _defaultListName = "MARLOHN CHOINSKI";

        public MailService(IMailchimpClient mailchimpClient)
        {
            _mailchimpClient = mailchimpClient;
        }

        public async Task AddContactsAsync(IEnumerable<Contact> contacts)
        {
            string listId = await GetDefaultListId();

            foreach (var contact in contacts)
            {
                var member = await _mailchimpClient.AddMemberAsync(listId, new MailchimpMember
                {
                    EmailAddress = contact.Email,
                    Status = "subscribed",
                    MergeFields = new MailchimpMergeFields
                    {
                        FNAME = contact.FirstName,
                        LNAME = contact.LastName
                    }
                });
            }
        }

        private async Task<string> GetDefaultListId()
        {
            ListResponse listResponse = await _mailchimpClient.GetLists();

            // Mailchimp's free plan allows only one list and does not permit the removal of the default list
            MailchimpList defaultList = listResponse.Lists.Single();

            if (defaultList.Name != _defaultListName)
            {
                defaultList.Name = _defaultListName;
                defaultList = await _mailchimpClient.UpdateList(defaultList.Id, defaultList);
            }

            if (defaultList.Name != _defaultListName)
            {
                throw new InvalidOperationException("Failed to set the default list name.");
            }

            return defaultList.Id;
        }
    }
}