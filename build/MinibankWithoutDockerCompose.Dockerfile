FROM mcr.microsoft.com/dotnet/sdk:5.0 AS src
WORKDIR /src
COPY . .

RUN dotnet build Minibank.Web -c Release -r linux-x64

RUN dotnet test Minibank.Core.Tests --no-build

RUN dotnet publish Minibank.Web -c Release -r linux-x64 --no-build -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as final 
WORKDIR /app

COPY --from=src /dist .

#ENV PostgresConnectionString = "Host=host.docker.internal;Port=5432;Database=minibank;Username=postgres;Password=123456"

# Нужно изменить значение Host в PostgresConnectionString в appsettings.json с localhost на host.docker.internal 
# Эта строка работает только, если dockerfile находится в той же директории, что и проекты и COPY должен быть запущен 
# не из context'а docker-compose'а, если я всё правильно понял

ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:5001
EXPOSE 5001

ENTRYPOINT ["dotnet", "Minibank.Web.dll"]
#docker build -f build\MiniBank.Dockerfile -t minibank:1.0.0.0 src\.
#docker run -d -p 5001:5001 --name minibank minibank:1.0.0.0

