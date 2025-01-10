using MailSyncer.Domain.Entities;
using MailSyncer.Infrastructure.Adapters.Mailchimp.Models;

namespace MailSyncer.Infrastructure.Adapters.Mailchimp
{
    public interface IMailchimpClient
    {
        Task<ListResponse> GetLists();
        Task<MailchimpList> UpdateList(string listId, MailchimpList list);
        Task<MailchimpMember> AddMemberAsync(string listId, MailchimpMember member);
    }
}