using MailSyncer.Domain.Entities;
using MailSyncer.Tests.Faker;

namespace MailSyncer.Tests.Tests.Domain
{
    public class ContactTests
    {
        [Fact]
        public void Validate_ShouldReturnNoErrorsWhenContactIsValid()
        {
            // Arrange
            var contact = ContactFaker.GenerateContacts(1).First();

            // Act
            var result = Contact.Validate(contact);

            // Assert
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Validate_ShouldReturnErrorWhenFirstNameIsEmpty()
        {
            // Arrange
            var contact = ContactFaker.GenerateContacts(1).First();
            contact.FirstName = "";

            // Act
            var result = Contact.Validate(contact);

            // Assert
            Assert.Contains("First name is required.", result.Errors);
        }

        [Fact]
        public void Validate_ShouldReturnErrorWhenLastNameIsEmpty()
        {
            // Arrange
            var contact = ContactFaker.GenerateContacts(1).First();
            contact.LastName = "";

            // Act
            var result = Contact.Validate(contact);

            // Assert
            Assert.Contains("Last name is required.", result.Errors);
        }

        [Fact]
        public void Validate_ShouldReturnErrorWhenEmailIsInvalid()
        {
            // Arrange
            var contact = ContactFaker.GenerateContacts(1).First();
            contact.Email = "invalid-email";

            // Act
            var result = Contact.Validate(contact);

            // Assert
            Assert.Contains("A valid email address is required.", result.Errors);
        }
    }
}
