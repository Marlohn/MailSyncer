using System.ComponentModel.DataAnnotations;
using MailSyncer.Application.Services;
using MailSyncer.Domain.Entities;
using MailSyncer.Domain.Interfaces;
using MailSyncer.Tests.Faker;
using Moq;

namespace MailSyncer.Tests.Tests.Application
{
    public class ContactSyncServiceTests
    {
        private readonly Mock<IContactService> _contactServiceMock;
        private readonly Mock<IMailService> _mailServiceMock;
        private readonly ContactSyncService _contactSyncService;

        public ContactSyncServiceTests()
        {
            _contactServiceMock = new Mock<IContactService>();
            _mailServiceMock = new Mock<IMailService>();
            _contactSyncService = new ContactSyncService(_contactServiceMock.Object, _mailServiceMock.Object);
        }

        [Fact]
        public async Task SyncContactsAsync_ShouldReturnSyncedContacts_WhenContactsAreValid()
        {
            // Arrange
            var contacts = ContactFaker.GenerateContacts(5);

            _contactServiceMock.Setup(cs => cs.GetContactsAsync())
                .ReturnsAsync(contacts);

            var syncResult = new SyncContactsResult
            {
                SuccessContacts = contacts
            };

            _mailServiceMock.Setup(ms => ms.SyncContactsAsync(contacts)).ReturnsAsync(syncResult);

            // Act
            var response = await _contactSyncService.SyncContactsAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(contacts.Count, response.SyncedContacts);
            Assert.Equal(contacts.Count, response.Contacts.Count);
        }

        [Fact]
        public async Task SyncContactsAsync_ShouldReturnEmptyResponse_WhenNocontacts()
        {
            // Arrange
            var incontacts = new List<Contact>();

            _contactServiceMock.Setup(cs => cs.GetContactsAsync()).ReturnsAsync(incontacts);

            // Act
            var response = await _contactSyncService.SyncContactsAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(0, response.SyncedContacts);
            Assert.Empty(response.Contacts);
        }

        [Fact]
        public async Task SyncContactsAsync_ShouldHandleExceptionsFromContactService()
        {
            // Arrange
            _contactServiceMock.Setup(cs => cs.GetContactsAsync()).ThrowsAsync(new Exception("Service error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(_contactSyncService.SyncContactsAsync);
        }

        [Fact]
        public async Task SyncContactsAsync_ShouldHandleValidationExceptionIncontacts()
        {
            // Arrange
            var contacts = ContactFaker.GenerateContacts(3);
            var invalidContact = new Contact { Email = "invalid-email" };

            contacts.Add(invalidContact);

            _contactServiceMock.Setup(cs => cs.GetContactsAsync()).ReturnsAsync(contacts);

            var syncResult = new SyncContactsResult
            {
                SuccessContacts = contacts
            };

            _mailServiceMock.Setup(ms => ms.SyncContactsAsync(It.IsAny<List<Contact>>())).ReturnsAsync(syncResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(_contactSyncService.SyncContactsAsync);

        }

        [Fact]
        public async Task GetContactsAsync_ShouldReturnContactsFromMailService()
        {
            // Arrange
            var contacts = ContactFaker.GenerateContacts(3);

            var syncResult = new SyncContactsResult
            {
                SuccessContacts = contacts
            };

            _mailServiceMock.Setup(ms => ms.GetContactsAsync()).ReturnsAsync(syncResult);

            // Act
            var response = await _contactSyncService.GetContactsAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(contacts.Count, response.SyncedContacts);
            Assert.Equal(contacts.Count, response.Contacts.Count);
        }

        [Fact]
        public async Task CleanContactsAsync_ShouldReturnCleanedContactsFromMailService()
        {
            // Arrange
            var contacts = ContactFaker.GenerateContacts(2);

            var syncResult = new SyncContactsResult
            {
                SuccessContacts = contacts
            };

            _mailServiceMock.Setup(ms => ms.CleanContactsAsync()).ReturnsAsync(syncResult);

            // Act
            var response = await _contactSyncService.CleanContactsAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(contacts.Count, response.SyncedContacts);
            Assert.Equal(contacts.Count, response.Contacts.Count);
        }
    }
}
