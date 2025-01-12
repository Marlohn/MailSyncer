# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/MailSyncer.API/MailSyncer.API.csproj", "Presentation/MailSyncer.API/"]
COPY ["Infrastructure/MailSyncer.IoC/MailSyncer.IoC.csproj", "Infrastructure/MailSyncer.IoC/"]
COPY ["Application/MailSyncer.Application/MailSyncer.Application.csproj", "Application/MailSyncer.Application/"]
COPY ["Domain/MailSyncer.Domain/MailSyncer.Domain.csproj", "Domain/MailSyncer.Domain/"]
COPY ["Infrastructure/MailSyncer.Infrastructure/MailSyncer.Infrastructure.csproj", "Infrastructure/MailSyncer.Infrastructure/"]
RUN dotnet restore "./Presentation/MailSyncer.API/MailSyncer.API.csproj"
COPY . .
WORKDIR "/src/Presentation/MailSyncer.API"
RUN dotnet build "./MailSyncer.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MailSyncer.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MailSyncer.API.dll"]