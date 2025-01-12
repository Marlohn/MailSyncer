namespace MailSyncer.Domain.Entities
{
    public class SyncContactsResult
    {
        public List<Contact> SuccessContacts { get; set; } = [];
        public List<Contact> FailedContacts { get; set; } = [];
        public int SyncedContacts { get; set; }
    }
}