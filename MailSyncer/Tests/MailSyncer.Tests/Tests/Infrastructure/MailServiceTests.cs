using MailSyncer.Infrastructure.Adapters.Mailchimp;
using MailSyncer.Infrastructure.Adapters.Mailchimp.Models;
using MailSyncer.Infrastructure.HttpClients.Models;
using MailSyncer.Infrastructure.Services;
using MailSyncer.Tests.Faker;
using Moq;

namespace MailSyncer.Tests.Tests.Infrastructure
{
    public class MailServiceTests
    {
        private readonly Mock<IMailchimpClient> _mailchimpClientMock;
        private readonly MailService _mailService;
        private const string DefaultListName = "MARLOHN CHOINSKI";
        private const string DefaultListId = "1";

        public MailServiceTests()
        {
            _mailchimpClientMock = new Mock<IMailchimpClient>();
            _mailService = new MailService(_mailchimpClientMock.Object);
        }

        [Fact]
        public async Task SyncContactsAsync_ShouldAddContactsToMailchimp()
        {
            // Arrange
            var contacts = ContactFaker.GenerateContacts(2);

            var mailchimpMember = new MailchimpMember
            {
                Id = string.Empty,
                EmailAddress = "john.doe@example.com",
                Status = "subscribed",
                MergeFields = new MailchimpMergeFields { FName = "John", LName = "Doe" }
            };

            _mailchimpClientMock.Setup(x => x.GetLists())
                .ReturnsAsync(ResponseWrapper<MailchimpLists>.Success(new MailchimpLists
                {
                    Lists = new List<MailchimpList> { new MailchimpList { Id = DefaultListId, Name = DefaultListName } }
                }));

            _mailchimpClientMock.Setup(x => x.AddMemberAsync(DefaultListId, It.IsAny<MailchimpMember>()))
                .ReturnsAsync(ResponseWrapper<MailchimpMember>.Success(mailchimpMember));

            // Act
            var result = await _mailService.SyncContactsAsync(contacts);

            // Assert
            Assert.Equal(2, result.SuccessContacts.Count);
            Assert.Empty(result.FailedContacts);
        }

        [Fact]
        public async Task GetContactsAsync_ShouldReturnContactsFromMailchimp()
        {
            // Arrange
            var mailchimpMembers = new MailchimpMembers
            {
                Members = new List<MailchimpMember>
                    {
                        new MailchimpMember
                        {
                            Id = "1",
                            EmailAddress = "john.doe@example.com",
                            Status = "subscribed",
                            MergeFields = new MailchimpMergeFields { FName = "John", LName = "Doe" }
                        }
                    }
            };

            _mailchimpClientMock.Setup(x => x.GetLists())
                .ReturnsAsync(ResponseWrapper<MailchimpLists>.Success(new MailchimpLists
                {
                    Lists = new List<MailchimpList> { new MailchimpList { Id = DefaultListId, Name = DefaultListName } }
                }));

            _mailchimpClientMock.Setup(x => x.GetMembersAsync(DefaultListId))
                .ReturnsAsync(ResponseWrapper<MailchimpMembers>.Success(mailchimpMembers));

            // Act
            var result = await _mailService.GetContactsAsync();

            // Assert
            Assert.Single(result.SuccessContacts);
            Assert.Empty(result.FailedContacts);
        }

        [Fact]
        public async Task CleanContactsAsync_ShouldDeleteContactsFromMailchimp()
        {
            // Arrange
            var mailchimpMembers = new MailchimpMembers
            {
                Members = new List<MailchimpMember>
                    {
                        new MailchimpMember
                        {
                            Id = "1",
                            EmailAddress = "john.doe@example.com",
                            Status = "subscribed",
                            MergeFields = new MailchimpMergeFields { FName = "John", LName = "Doe" }
                        }
                    }
            };

            _mailchimpClientMock.Setup(x => x.GetLists())
                .ReturnsAsync(ResponseWrapper<MailchimpLists>.Success(new MailchimpLists
                {
                    Lists = new List<MailchimpList> { new MailchimpList { Id = DefaultListId, Name = DefaultListName } }
                }));

            _mailchimpClientMock.Setup(x => x.GetMembersAsync(DefaultListId))
                .ReturnsAsync(ResponseWrapper<MailchimpMembers>.Success(mailchimpMembers));

            _mailchimpClientMock.Setup(x => x.DeleteMemberAsync(DefaultListId, "1"))
                .ReturnsAsync(ResponseWrapper.Success());

            // Act
            var result = await _mailService.CleanContactsAsync();

            // Assert
            Assert.Single(result.SuccessContacts);
            Assert.Empty(result.FailedContacts);
        }

        [Fact]
        public async Task SyncContactsAsync_ShouldHandleFailureToAddContactsToMailchimp()
        {
            // Arrange
            var contacts = ContactFaker.GenerateContacts(2);

            _mailchimpClientMock.Setup(x => x.GetLists())
                .ReturnsAsync(ResponseWrapper<MailchimpLists>.Success(new MailchimpLists
                {
                    Lists = new List<MailchimpList> { new MailchimpList { Id = DefaultListId, Name = DefaultListName } }
                }));

            _mailchimpClientMock.Setup(x => x.AddMemberAsync(DefaultListId, It.IsAny<MailchimpMember>()))
                .ReturnsAsync(ResponseWrapper<MailchimpMember>.Fail("Failed to add member"));

            // Act
            var result = await _mailService.SyncContactsAsync(contacts);

            // Assert
            Assert.Empty(result.SuccessContacts);
            Assert.Equal(2, result.FailedContacts.Count);
        }

        [Fact]
        public async Task GetContactsAsync_ShouldHandleFailureToGetListsFromMailchimp()
        {
            // Arrange
            string errorMessage = "Failed to get lists";
            _mailchimpClientMock.Setup(x => x.GetLists())
                .ReturnsAsync(ResponseWrapper<MailchimpLists>.Fail(errorMessage));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(_mailService.GetContactsAsync);
            Assert.Equal(errorMessage, exception.Message);
        }

        [Fact]
        public async Task GetContactsAsync_ShouldHandleFailureToGetMembersFromMailchimp()
        {
            // Arrange
            _mailchimpClientMock.Setup(x => x.GetLists())
                .ReturnsAsync(ResponseWrapper<MailchimpLists>.Success(new MailchimpLists
                {
                    Lists = new List<MailchimpList> { new MailchimpList { Id = DefaultListId, Name = DefaultListName } }
                }));

            _mailchimpClientMock.Setup(x => x.GetMembersAsync(DefaultListId))
                .ReturnsAsync(ResponseWrapper<MailchimpMembers>.Fail("Failed to get members"));

            // Act
            var result = await _mailService.GetContactsAsync();

            // Assert
            Assert.Empty(result.SuccessContacts);
            Assert.Empty(result.FailedContacts);
        }
    }
}