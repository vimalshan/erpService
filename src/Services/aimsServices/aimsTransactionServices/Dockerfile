FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5122
ENV ASPNETCORE_URLS=http://+:5122

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/AimsTransactionService.API/AimsTransactionService.API.csproj AimsTransactionService.API/
COPY src/AimsTransactionService.Application/AimsTransactionService.Application.csproj AimsTransactionService.Application/
COPY src/AimsTransactionService.Domain/AimsTransactionService.Domain.csproj AimsTransactionService.Domain/
COPY src/AimsTransactionService.Infrastructure/AimsTransactionService.Infrastructure.csproj AimsTransactionService.Infrastructure/
RUN dotnet restore AimsTransactionService.API/AimsTransactionService.API.csproj

COPY src/ .
RUN find . -name 'bin' -o -name 'obj' | xargs rm -rf 2>/dev/null || true
RUN dotnet restore AimsTransactionService.API/AimsTransactionService.API.csproj
WORKDIR /src/AimsTransactionService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "AimsTransactionService.API.dll"]
