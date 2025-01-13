using System.Net.Http.Headers;
using System.Text;
using MailSyncer.Application.Interfaces;
using MailSyncer.Application.Services;
using MailSyncer.Domain.Interfaces;
using MailSyncer.Domain.Interfaces.Authentication;
using MailSyncer.Infrastructure.Adapters.Mailchimp;
using MailSyncer.Infrastructure.Adapters.Mailchimp.Settings;
using MailSyncer.Infrastructure.Adapters.MockApi;
using MailSyncer.Infrastructure.HttpClients.Settings;
using MailSyncer.Infrastructure.Services;
using MailSyncer.Infrastructure.Services.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MailSyncer.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSettings(configuration);
            services.AddApplication();
            services.AddInfrastructure();
            services.AddHttpClients();
            services.AddAuthentication();

            return services;
        }

        private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailchimpSettings>(configuration.GetSection("Mailchimp"));
            services.Configure<MockApiSettings>(configuration.GetSection("MockApi"));

            return services;
        }

        private static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IContactSyncService, ContactSyncService>();
            services.AddScoped<IAuthenticationServiceApplication, AuthenticationServiceApplication>();

            return services;
        }

        private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<MailchimpSettings>>().Value);
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<MockApiSettings>>().Value);

            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IMailService, MailService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();

            return services;
        }

        private static IServiceCollection AddHttpClients(this IServiceCollection services)
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

        private static IServiceCollection AddAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = "api-audience",
                        ValidIssuer = "api-issuer",
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("minha-chave-secreta-super-segura")),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("UserPolicy", policy =>
                    policy.RequireRole("User"));
            });

            return services;
        }
    }
}