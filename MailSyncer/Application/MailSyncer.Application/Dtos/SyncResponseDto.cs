using MailSyncer.Domain.Entities;

namespace MailSyncer.Application.Dtos
{
    public class SyncResponseDto
    {
        public int SyncedContacts { get; set; }
        public List<Contact> Members { get; set; }
    }
}