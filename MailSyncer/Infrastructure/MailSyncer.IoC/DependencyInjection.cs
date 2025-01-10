using System.Net.Http.Headers;
using MailSyncer.Application.Interfaces;
using MailSyncer.Application.Services;
using MailSyncer.Domain.Interfaces;
using MailSyncer.Infrastructure.Adapters.Mailchimp;
using MailSyncer.Infrastructure.Adapters.MockApi;
using MailSyncer.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MailSyncer.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure();
            services.AddHttpClients();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IContactSyncService, ContactSyncService>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IMailService, MailService>();

            return services;
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IMockApiClient, MockApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://challenge.trio.dev/api/v1/");
                //client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<IMailchimpClient, MailchimpClient>(client =>
            {
                client.BaseAddress = new Uri("https://us8.api.mailchimp.com/3.0/");
                //client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", "55c35dbdbe8a26cd71df9245ee3ad54b-us8");
            });


            //services.AddHttpClient<IMailService, MailchimpService>(client =>
            //{
            //    client.BaseAddress = new Uri("https://us8.api.mailchimp.com/3.0/");
            //    client.DefaultRequestHeaders.Authorization =
            //        new AuthenticationHeaderValue("Bearer", "55c35dbdbe8a26cd71df9245ee3ad54b-us8");
            //    //client.Timeout = TimeSpan.FromSeconds(30);
            //});

            return services;
        }
    }
}