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

        public async Task<SyncResponseDto> SyncContactsAsync()
        {
            var contacts = await _contactService.GetContactsAsync();

            var result = await _mailService.SyncContactsAsync(contacts);

            return new SyncResponseDto
            {
                SyncedContacts = result.SuccessContacts.Count,
                Contacts = result.SuccessContacts.Select(ContactDTO.Map).ToList()
            };
        }

        public async Task<SyncResponseDto> GetContactsAsync()
        {
            var result = await _mailService.GetContactsAsync();

            return new SyncResponseDto
            {
                SyncedContacts = result.SuccessContacts.Count,
                Contacts = result.SuccessContacts.Select(ContactDTO.Map).ToList()
            };
        }

        public async Task<SyncResponseDto> CleanContactsAsync()
        {
            var result = await _mailService.CleanContactsAsync();

            return new SyncResponseDto
            {
                SyncedContacts = result.SuccessContacts.Count,
                Contacts = result.SuccessContacts.Select(ContactDTO.Map).ToList()
            };
        }
    }
}
