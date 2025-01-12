using MailSyncer.Domain.Entities;

namespace MailSyncer.Application.Dtos
{
    public class ContactDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public static ContactDTO Map(Contact contact)
        {
            return new ContactDTO()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email
            };
        }
    }
}