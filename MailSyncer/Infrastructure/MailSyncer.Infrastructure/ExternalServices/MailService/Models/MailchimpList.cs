using System.Text.Json.Serialization;

namespace MailSyncer.Infrastructure.ExternalServices.MailService.Models
{
    internal class MailchimpList
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("permission_reminder")]
        public string PermissionReminder { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("campaign_defaults")]
        public MailchimpCampaignDefaults CampaignDefaults { get; set; }

        [JsonPropertyName("contact")]
        public MailchimpContact Contact { get; set; }

        [JsonPropertyName("email_type_option")]
        public bool EmailTypeOption { get; set; }
    }
}
