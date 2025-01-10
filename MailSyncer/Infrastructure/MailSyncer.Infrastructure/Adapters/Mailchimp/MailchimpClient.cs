using System.Collections.Generic;
using MailSyncer.Domain.Entities;
using MailSyncer.Infrastructure.Adapters.Mailchimp.Models;

namespace MailSyncer.Infrastructure.Adapters.Mailchimp
{
    public class MailchimpClient : BaseHttpClient, IMailchimpClient
    {
        
        public MailchimpClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ListResponse> GetLists()
        {
            return await GetAsync<ListResponse>("lists");
        }

        public async Task<MailchimpList> UpdateList(string listId, MailchimpList list)
        {
            return await PatchAsync<MailchimpList, MailchimpList>($"lists/{listId}", list);
        }

        public async Task<MailchimpMember> AddMemberAsync(string listId, MailchimpMember member)
        {
            //check if this returns is enounght
            return await PostAsync<MailchimpMember, MailchimpMember>($"lists/{listId}/members", member);
        }

        //public async Task<List<Contact>> GetContactsAsync()
        //{
        //    return await GetAsync<List<Contact>>("contacts");
        //}

        //public async Task<Contact> CreateContactAsync(Contact newContact)
        //{
        //    return await PostAsync<Contact, Contact>("contacts", newContact);
        //}

        //public async Task UpdateContactAsync(string contactId, Contact updatedContact)
        //{
        //    await PutAsync<Contact, object>($"contacts/{contactId}", updatedContact);
        //}

        //public async Task DeleteContactAsync(string contactId)
        //{
        //    await DeleteAsync($"contacts/{contactId}");
        //}
    }
}