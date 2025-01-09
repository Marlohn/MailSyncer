using MailSyncer.Application.Dtos;
using MailSyncer.Application.Interfaces;
using MailSyncer.Domain.Interfaces;

namespace MailSyncer.Application.Services
{
    public class ContactSyncService : IContactSyncService
    {
        private readonly IContactService _contactService;
        private readonly IMailService _mailService;

        public ContactSyncService(IContactService contactService, IMailService mailService)
        {
            _contactService = contactService;
            _mailService = mailService;
        }

        public async Task SyncContactsAsync()
        {
            var contacts = await _contactService.GetContactsAsync();

            await _mailService.AddContactsAsync(contacts);
        }
    }
}
