namespace MailSyncer.Application.Dtos
{
    public class SyncResponseDto
    {
        public int SyncedContacts { get; set; }
        public List<ContactDTO> Contacts { get; set; }
    }
}