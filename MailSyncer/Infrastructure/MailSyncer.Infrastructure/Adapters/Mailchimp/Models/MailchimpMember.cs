using System.Text.Json.Serialization;
using MailSyncer.Domain.Entities;

namespace MailSyncer.Infrastructure.Adapters.Mailchimp.Models
{
    public class MailchimpMember
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("email_address")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("merge_fields")]
        public MailchimpMergeFields MergeFields { get; set; }

        public static Contact Map(MailchimpMember member)
        {
            return new Contact()
            {
                Email = member.EmailAddress,
                FirstName = member.MergeFields.FName,
                LastName = member.MergeFields.LName
            };
        }

    }
}