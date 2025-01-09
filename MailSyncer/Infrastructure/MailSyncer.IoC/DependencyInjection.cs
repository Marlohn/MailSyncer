using System.Net.Http.Headers;
using MailSyncer.Application.Interfaces;
using MailSyncer.Application.Services;
using MailSyncer.Domain.Interfaces;
using MailSyncer.Infrastructure.ExternalServices.ContactService;
using MailSyncer.Infrastructure.ExternalServices.MailService;
using Microsoft.Extensions.DependencyInjection;

namespace MailSyncer.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            // Infrastructure
            services.AddHttpClient<IContactService, MockApiService>(client =>
            {
                client.BaseAddress = new Uri("https://challenge.trio.dev/api/v1/");
                //client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<IMailService, MailchimpService>(client =>
            {
                client.BaseAddress = new Uri("https://us8.api.mailchimp.com/3.0/");
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", "55c35dbdbe8a26cd71df9245ee3ad54b-us8");
                //client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Application
            services.AddScoped<IContactSyncService, ContactSyncService>();

            return services;
        }
    }
}