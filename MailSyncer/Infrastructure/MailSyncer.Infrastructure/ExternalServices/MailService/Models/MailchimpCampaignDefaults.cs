using System.Text.Json.Serialization;

namespace MailSyncer.Infrastructure.ExternalServices.MailService.Models
{
    internal class MailchimpCampaignDefaults
    {
        [JsonPropertyName("from_email")]
        public string FromEmail { get; set; }

        [JsonPropertyName("from_name")]
        public string FromName { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }
    }
}