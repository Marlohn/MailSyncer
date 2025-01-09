using System.Text.Json.Serialization;

namespace MailSyncer.Infrastructure.ExternalServices.MailService.Models
{
    internal class ListResponse
    {
        [JsonPropertyName("lists")]
        public List<MailchimpList> Lists { get; set; }
    }
}