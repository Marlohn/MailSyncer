using MailSyncer.Infrastructure.Adapters.Mailchimp.Models;
using MailSyncer.Infrastructure.HttpClients.Models;

namespace MailSyncer.Infrastructure.Adapters.Mailchimp
{
    public interface IMailchimpClient
    {
        Task<ResponseWrapper<MailchimpLists>> GetLists();
        Task<ResponseWrapper<MailchimpList>> UpdateList(string listId, MailchimpList list);
        Task<ResponseWrapper<MailchimpMembers>> GetMembersAsync(string listId);
        Task<ResponseWrapper<MailchimpMember>> AddMemberAsync(string listId, MailchimpMember member);
    }
}