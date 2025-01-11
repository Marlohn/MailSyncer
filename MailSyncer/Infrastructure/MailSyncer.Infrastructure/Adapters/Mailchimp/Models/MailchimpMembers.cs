using System.Text.Json.Serialization;

namespace MailSyncer.Infrastructure.Adapters.Mailchimp.Models
{
    public class MailchimpMembers
    {
        [JsonPropertyName("members")]
        public List<MailchimpMember> Members { get; set; }
    }
}