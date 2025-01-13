using MailSyncer.API.Middlewares;
using MailSyncer.Infrastructure.Adapters.Mailchimp.Settings;
using MailSyncer.Infrastructure.HttpClients.Settings;
using MailSyncer.IoC;
using Scalar.AspNetCore;

namespace MailSyncer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddOpenApi();

            // Register settings
            builder.Services.Configure<MailchimpSettings>(builder.Configuration.GetSection("Mailchimp"));
            builder.Services.Configure<MockApiSettings>(builder.Configuration.GetSection("MockApi"));

            // Add services to the container.

            builder.Services.AddDependencies();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/scalar/v1");
                    return;
                }
                await next();
            });

            app.MapOpenApi();
            app.MapScalarApiReference();


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
