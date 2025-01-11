using System.Text.Json.Serialization;

namespace MailSyncer.Infrastructure.Adapters.Mailchimp.Models
{
    public class MailchimpMergeFields
    {
        [JsonPropertyName("FNAME")]
        public string FName { get; set; }

        [JsonPropertyName("LNAME")]
        public string LName { get; set; }
    }
}