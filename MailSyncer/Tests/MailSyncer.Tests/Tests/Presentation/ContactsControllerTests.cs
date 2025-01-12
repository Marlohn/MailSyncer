using MailSyncer.API.Controllers;
using MailSyncer.Application.Dtos;
using MailSyncer.Application.Interfaces;
using MailSyncer.UnitTests.Faker;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MailSyncer.UnitTests.Tests.Presentation
{
    public class ContactsControllerTests
    {
        private readonly Mock<IContactSyncService> _contactSyncServiceMock;
        private readonly ContactsController _controller;

        public ContactsControllerTests()
        {
            _contactSyncServiceMock = new Mock<IContactSyncService>();
            _controller = new ContactsController(_contactSyncServiceMock.Object);
        }

        [Fact]
        public async Task SyncContacts_ShouldReturnOk_WithFakeContacts()
        {
            // Arrange
            var fakeContacts = ContactDtoFaker.GenerateContacts(5);

            var syncResponse = new SyncResponseDto
            {
                SyncedContacts = 5,
                Contacts = fakeContacts
            };

            _contactSyncServiceMock.Setup(s => s.SyncContactsAsync()).ReturnsAsync(syncResponse);

            // Act
            var result = await _controller.SyncContacts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(syncResponse, okResult.Value);
        }

        [Fact]
        public async Task SyncContacts_ShouldReturnNoContent_WhenNoContactsSynced()
        {
            // Arrange
            var syncResponse = new SyncResponseDto
            {
                SyncedContacts = 0,
                Contacts = new List<ContactDTO>()
            };

            _contactSyncServiceMock.Setup(s => s.SyncContactsAsync()).ReturnsAsync(syncResponse);

            // Act
            var result = await _controller.SyncContacts();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetContacts_ShouldReturnOk_WithFakeContacts()
        {
            // Arrange
            var fakeContacts = ContactDtoFaker.GenerateContacts(3);

            var syncResponse = new SyncResponseDto
            {
                SyncedContacts = 3,
                Contacts = fakeContacts
            };

            _contactSyncServiceMock.Setup(s => s.GetContactsAsync()).ReturnsAsync(syncResponse);

            // Act
            var result = await _controller.GetContacts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(syncResponse, okResult.Value);
        }

        [Fact]
        public async Task GetContacts_ShouldReturnNoContent_WhenNoContactsRetrieved()
        {
            // Arrange
            var syncResponse = new SyncResponseDto
            {
                SyncedContacts = 0,
                Contacts = new List<ContactDTO>()
            };

            _contactSyncServiceMock.Setup(s => s.GetContactsAsync()).ReturnsAsync(syncResponse);

            // Act
            var result = await _controller.GetContacts();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task CleanContacts_ShouldReturnOk_WithFakeContacts()
        {
            // Arrange
            var fakeContacts = ContactDtoFaker.GenerateContacts(10);

            var syncResponse = new SyncResponseDto
            {
                SyncedContacts = 10,
                Contacts = fakeContacts
            };

            _contactSyncServiceMock.Setup(s => s.CleanContactsAsync()).ReturnsAsync(syncResponse);

            // Act
            var result = await _controller.CleanContacts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(syncResponse, okResult.Value);
        }

        [Fact]
        public async Task CleanContacts_ShouldReturnNoContent_WhenNoContactsCleaned()
        {
            // Arrange
            var syncResponse = new SyncResponseDto
            {
                SyncedContacts = 0,
                Contacts = new List<ContactDTO>()
            };

            _contactSyncServiceMock.Setup(s => s.CleanContactsAsync()).ReturnsAsync(syncResponse);

            // Act
            var result = await _controller.CleanContacts();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}