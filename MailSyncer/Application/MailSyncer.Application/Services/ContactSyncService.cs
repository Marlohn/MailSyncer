using System.ComponentModel.DataAnnotations;
using MailSyncer.Application.Dtos;
using MailSyncer.Application.Interfaces;
using MailSyncer.Domain.Entities;
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
            List<Contact> validContacts = await GetValidContacts();

            if (validContacts.Count == 0)
                return new SyncResponseDto();

            SyncContactsResult result = await _mailService.SyncContactsAsync(validContacts);

            return new SyncResponseDto
            {
                SyncedContacts = result.SuccessContacts.Count,
                Contacts = result.SuccessContacts.Select(ContactDTO.Map).ToList()
            };
        }

        public async Task<SyncResponseDto> GetContactsAsync()
        {
            SyncContactsResult result = await _mailService.GetContactsAsync();

            return new SyncResponseDto
            {
                SyncedContacts = result.SuccessContacts.Count,
                Contacts = result.SuccessContacts.Select(ContactDTO.Map).ToList()
            };
        }

        public async Task<SyncResponseDto> CleanContactsAsync()
        {
            SyncContactsResult result = await _mailService.CleanContactsAsync();

            return new SyncResponseDto
            {
                SyncedContacts = result.SuccessContacts.Count,
                Contacts = result.SuccessContacts.Select(ContactDTO.Map).ToList()
            };
        }

        private async Task<List<Contact>> GetValidContacts()
        {
            var contacts = await _contactService.GetContactsAsync();

            var validContacts = new List<Contact>();

            foreach (var contact in contacts)
            {
                var validationResult = Contact.Validate(contact);

                if (validationResult.IsValid)
                {
                    validContacts.Add(contact);
                }
                else
                {
                    // TODO: Log the errors and allow the process to continue without interruption or find a way to return the errors to api
                    throw new ValidationException($"Invalid contact: {contact.Email}. Errors: {string.Join(", ", validationResult.Errors)}");
                }
            }

            return validContacts;
        }
    }
}