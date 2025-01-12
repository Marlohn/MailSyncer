FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copia o arquivo de solução e os arquivos de projeto (.csproj)
COPY MailSyncer.sln ./
COPY Application/Application.csproj ./Application/
COPY Domain/Domain.csproj ./Domain/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY Mailchimp.Connector/Mailchimp.Connector.csproj ./Mailchimp.Connector/
COPY Presentation/Presentation.csproj ./Presentation/
COPY Shared/Shared.csproj ./Shared/
COPY Tests/Tests.csproj ./Tests/

# Restaura as dependências
RUN dotnet restore

# Copia todo o código-fonte restante
COPY . ./

# Publica o projeto principal
RUN dotnet publish Presentation/Presentation.csproj -c Release -o /app/publish

# Cria a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/publish .

# Define a porta exposta
EXPOSE 5000

# Define o ponto de entrada
ENTRYPOINT ["dotnet", "Presentation.dll"]