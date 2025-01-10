using System.Text.Json.Serialization;

namespace MailSyncer.Infrastructure.Adapters.Mailchimp.Models
{
    public class ListResponse
    {
        [JsonPropertyName("lists")]
        public List<MailchimpList> Lists { get; set; }
    }
}