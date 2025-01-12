using System.Net.Http.Headers;
using MailSyncer.Application.Interfaces;
using MailSyncer.Application.Services;
using MailSyncer.Domain.Interfaces;
using MailSyncer.Infrastructure.Adapters.Mailchimp;
using MailSyncer.Infrastructure.Adapters.Mailchimp.Settings;
using MailSyncer.Infrastructure.Adapters.MockApi;
using MailSyncer.Infrastructure.HttpClients.Settings;
using MailSyncer.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<MailchimpSettings>>().Value);
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<MockApiSettings>>().Value);

            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IMailService, MailService>();

            return services;
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IMockApiClient, MockApiClient>((provider, client) =>
            {
                var settings = provider.GetRequiredService<IOptions<MockApiSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
            });

            services.AddHttpClient<IMailchimpClient, MailchimpClient>((provider, client) =>
            {
                var settings = provider.GetRequiredService<IOptions<MailchimpSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", settings.ApiKey);
            });

            return services;
        }
    }
}