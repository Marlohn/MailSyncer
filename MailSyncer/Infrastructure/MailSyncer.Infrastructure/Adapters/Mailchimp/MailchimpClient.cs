using MailSyncer.Infrastructure.Adapters.Mailchimp.Models;
using MailSyncer.Infrastructure.HttpClients;
using MailSyncer.Infrastructure.HttpClients.Models;

namespace MailSyncer.Infrastructure.Adapters.Mailchimp
{
    public class MailchimpClient : HttpClientBase, IMailchimpClient
    {
        public MailchimpClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ResponseWrapper<MailchimpLists>> GetLists()
        {
            return await GetAsync<MailchimpLists>("lists");
        }

        public async Task<ResponseWrapper<MailchimpList>> UpdateList(string listId, MailchimpList list)
        {
            return await PatchAsync<MailchimpList, MailchimpList>($"lists/{listId}", list);
        }

        public async Task<ResponseWrapper<MailchimpMembers>> GetMembersAsync(string listId)
        {
            return await GetAsync<MailchimpMembers>($"lists/{listId}/members");
        }

        public async Task<ResponseWrapper<MailchimpMember>> AddMemberAsync(string listId, MailchimpMember member)
        {
            return await PostAsync<MailchimpMember, MailchimpMember>($"lists/{listId}/members", member);
        }

        public async Task<ResponseWrapper> DeleteMemberAsync(string listId, string memberId)
        {
            return await DeleteAsync($"lists/{listId}/members/{memberId}");
        }
    }
}