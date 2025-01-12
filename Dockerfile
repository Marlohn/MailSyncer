FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copia os arquivos de projeto e restaura dependências
COPY *.sln ./
COPY MailSyncer/* ./MailSyncer/
RUN dotnet restore

# Compila o projeto
RUN dotnet publish -c Release -o /app/publish

# Cria a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/publish .

# Porta exposta
EXPOSE 5000

# Comando para rodar o app
ENTRYPOINT ["dotnet", "MailSyncer.dll"]
