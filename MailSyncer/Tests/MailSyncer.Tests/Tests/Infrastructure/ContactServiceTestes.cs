using MailSyncer.Domain.Entities;
using MailSyncer.Infrastructure.Adapters.MockApi;
using MailSyncer.Infrastructure.HttpClients.Models;
using MailSyncer.Infrastructure.Services;
using MailSyncer.Tests.Faker;
using Moq;

namespace MailSyncer.Tests.Tests.Infrastructure
{
    public class ContactServiceTests
    {
        [Fact]
        public async Task GetContactsAsync_ShouldReturnContacts_WhenApiResponseIsSuccessful()
        {
            // Arrange
            var mockApiClient = new Mock<IMockApiClient>();
            var contacts = ContactFaker.GenerateContacts(5);

            mockApiClient.Setup(api => api.GetContactsAsync()).ReturnsAsync(ResponseWrapper<List<Contact>>.Success(contacts));

            var contactService = new ContactService(mockApiClient.Object);

            // Act
            var result = await contactService.GetContactsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contacts.Count, result.Count);
            Assert.Equal(contacts[0].Email, result[0].Email);
        }

        [Fact]
        public async Task GetContactsAsync_ShouldThrowException_WhenApiResponseIsUnsuccessful()
        {
            // Arrange
            string errorMessage = "Error fetching contacts";
            var mockApiClient = new Mock<IMockApiClient>();
            mockApiClient.Setup(api => api.GetContactsAsync()).ReturnsAsync(ResponseWrapper<List<Contact>>.Fail(errorMessage));

            var contactService = new ContactService(mockApiClient.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(contactService.GetContactsAsync);
            Assert.Equal(errorMessage, exception.Message);
        }

        [Fact]
        public async Task GetContactsAsync_ShouldReturnEmptyList_WhenApiResponseHasNoData()
        {
            // Arrange
            var mockApiClient = new Mock<IMockApiClient>();
            mockApiClient.Setup(api => api.GetContactsAsync()).ReturnsAsync(ResponseWrapper<List<Contact>>.Success(new List<Contact>()));

            var contactService = new ContactService(mockApiClient.Object);

            // Act
            var result = await contactService.GetContactsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}