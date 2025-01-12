using MailSyncer.Domain.Validators;

namespace MailSyncer.Domain.Entities
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public static ValidationResult Validate(Contact contact)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(contact.FirstName))
                result.Errors.Add("First name is required.");

            if (string.IsNullOrWhiteSpace(contact.LastName))
                result.Errors.Add("Last name is required.");

            if (string.IsNullOrWhiteSpace(contact.Email) || !contact.Email.Contains("@"))
                result.Errors.Add("A valid email address is required.");

            return result;
        }
    }
}