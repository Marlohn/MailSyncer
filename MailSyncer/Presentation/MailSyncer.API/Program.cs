using System.Text;
using MailSyncer.API.Middlewares;
using MailSyncer.Infrastructure.Adapters.Mailchimp.Settings;
using MailSyncer.Infrastructure.HttpClients.Settings;
using MailSyncer.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace MailSyncer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddOpenApi();
            builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

            // Add services to the container.
            builder.Services.AddDependencies(builder.Configuration);

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
            //app.MapScalarApiReference(options =>
            //{
            //    options.WithTheme(ScalarTheme.Moon)
            //        .WithDarkMode(true)             // Modo escuro ativado
            //        .WithDarkModeToggle(false)      // Oculta a alternância de modo escuro
            //        .WithPreferredScheme("Bearer")  // Define "Bearer" como esquema preferido
            //        .WithHttpBearerAuthentication(bearer =>
            //        {
            //            bearer.Token = "your-bearer-token"; // Aqui você pode configurar um token estático ou um dinâmico
            //        });

            //    options.Authentication = new ScalarAuthenticationOptions
            //    {
            //        PreferredSecurityScheme = "Bearer" // Esquema de segurança preferido
            //    };
            //});

            app.MapScalarApiReference(option => {
                option
                    .WithTitle("Auth API")
                    .WithTheme(ScalarTheme.DeepSpace)
                    .WithDownloadButton(true)
                    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
