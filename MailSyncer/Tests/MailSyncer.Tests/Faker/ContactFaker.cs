using Bogus;
using MailSyncer.Domain.Entities;

namespace MailSyncer.Tests.Faker
{
    public static class ContactFaker
    {
        public static List<Contact> GenerateContacts(int count)
        {
            var faker = new Faker<Contact>()
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, f => f.Internet.Email());

            return faker.Generate(count);
        }
    }
}