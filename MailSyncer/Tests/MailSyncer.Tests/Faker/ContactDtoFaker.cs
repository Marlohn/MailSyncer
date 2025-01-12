using Bogus;
using MailSyncer.Application.Dtos;

namespace MailSyncer.Tests.Faker
{
    public static class ContactDtoFaker
    {
        public static List<ContactDTO> GenerateContacts(int count)
        {
            var faker = new Faker<ContactDTO>()
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, f => f.Internet.Email());

            return faker.Generate(count);
        }
    }
}