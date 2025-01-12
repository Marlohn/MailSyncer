using MailSyncer.Domain.Entities;
using MailSyncer.Domain.Interfaces;
using MailSyncer.Infrastructure.Adapters.Mailchimp;
using MailSyncer.Infrastructure.Adapters.Mailchimp.Models;
using MailSyncer.Infrastructure.Adapters.Mailchimp.Settings;

namespace MailSyncer.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly IMailchimpClient _mailchimpClient;
        private readonly MailchimpSettings _mailchimpSettings;

        public MailService(IMailchimpClient mailchimpClient, MailchimpSettings mailchimpSettings)
        {
            _mailchimpClient = mailchimpClient;
            _mailchimpSettings = mailchimpSettings ?? throw new ArgumentNullException(nameof(mailchimpSettings), "Mailchimp settings cannot be null.");
        }

        public async Task<SyncContactsResult> SyncContactsAsync(List<Contact> contacts)
        {
            var syncContactsResult = new SyncContactsResult();

            string listId = await GetDefaultListId();

            foreach (var contact in contacts.Skip(0).Take(2))
            {
                var mailchimpMember = new MailchimpMember
                {
                    Id = string.Empty,
                    EmailAddress = contact.Email,
                    Status = "subscribed", //enum?
                    MergeFields = new MailchimpMergeFields
                    {
                        FName = contact.FirstName,
                        LName = contact.LastName
                    }

                    //EmailAddress = "example@gmail.com",
                    //Status = "subscribed", //enum?
                    //MergeFields = new MailchimpMergeFields
                    //{
                    //    FName = "John",
                    //    LName = "Doe"
                    //}                    

                };

                var member = await _mailchimpClient.AddMemberAsync(listId, mailchimpMember);

                if (member.IsSuccessfulWithData)
                {
                    syncContactsResult.SuccessContacts.Add(contact);
                }
                else
                {
                    syncContactsResult.FailedContacts.Add(contact);
                }
            }

            return syncContactsResult;
        }

        public async Task<SyncContactsResult> GetContactsAsync()
        {
            var syncContactsResult = new SyncContactsResult();

            string listId = await GetDefaultListId();

            var members = await _mailchimpClient.GetMembersAsync(listId);

            if (members.IsSuccessfulWithData)
            {
                foreach (var member in members.Data.Members)
                {
                    syncContactsResult.SuccessContacts.Add(MailchimpMember.Map(member));
                }
            }

            return syncContactsResult;
        }

        public async Task<SyncContactsResult> CleanContactsAsync()
        {
            var syncContactsResult = new SyncContactsResult();

            string listId = await GetDefaultListId();

            var members = await _mailchimpClient.GetMembersAsync(listId);

            if (members.IsSuccessfulWithData)
            {
                foreach (var member in members.Data.Members)
                {
                    var deleteResult = await _mailchimpClient.DeleteMemberAsync(listId, member.Id);

                    if (deleteResult.IsSuccessful)
                    {
                        syncContactsResult.SuccessContacts.Add(MailchimpMember.Map(member));
                    }
                    else
                    {
                        syncContactsResult.FailedContacts.Add(MailchimpMember.Map(member));
                    }
                }
            }

            return syncContactsResult;
        }

        private async Task<string> GetDefaultListId()
        {
            MailchimpList defaultList = await GetDefaultList();

            if (defaultList.Name != _mailchimpSettings.DefaultListName)
            {
                defaultList = await UpdateListToDefaultName(defaultList);
            }

            return defaultList.Id;
        }

        private async Task<MailchimpList> GetDefaultList()
        {
            var listsResponse = await _mailchimpClient.GetLists();

            if (!listsResponse.IsSuccessfulWithData)
            {
                throw new InvalidOperationException(listsResponse.ErrorMessage);
            }

            // Mailchimp's free plan allows only one list and does not permit the removal of the default list
            MailchimpList defaultList = listsResponse.Data.Lists.Single();

            return defaultList;
        }

        private async Task<MailchimpList> UpdateListToDefaultName(MailchimpList mailchimpList)
        {
            mailchimpList.Name = _mailchimpSettings.DefaultListName;

            var response = await _mailchimpClient.UpdateList(mailchimpList.Id, mailchimpList);

            if (!response.IsSuccessfulWithData)
            {
                throw new InvalidOperationException(response.ErrorMessage);
            }

            return response.Data;
        }
    }
}