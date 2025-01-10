using System.Text.Json.Serialization;

namespace MailSyncer.Infrastructure.Adapters.Mailchimp.Models
{
    public class MailchimpMember
    {
        [JsonPropertyName("email_address")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("merge_fields")]
        public MailchimpMergeFields MergeFields { get; set; }
    }
}