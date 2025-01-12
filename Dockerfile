FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copia os arquivos de solução e os projetos
COPY MailSyncer.sln ./
COPY MailSyncer/*.csproj ./MailSyncer/

# Restaura as dependências
RUN dotnet restore MailSyncer.sln

# Copia o restante dos arquivos do projeto
COPY MailSyncer/. ./MailSyncer/

# Compila o projeto
RUN dotnet publish MailSyncer.sln -c Release -o /app/publish

# Cria a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/publish .

# Porta exposta
EXPOSE 5000

# Comando para rodar o app
ENTRYPOINT ["dotnet", "MailSyncer.dll"]