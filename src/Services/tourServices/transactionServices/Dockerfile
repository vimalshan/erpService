FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/TransactionService.Domain/TransactionService.Domain.csproj src/TransactionService.Domain/
COPY src/TransactionService.Application/TransactionService.Application.csproj src/TransactionService.Application/
COPY src/TransactionService.Infrastructure/TransactionService.Infrastructure.csproj src/TransactionService.Infrastructure/
COPY src/TransactionService.API/TransactionService.API.csproj src/TransactionService.API/
RUN dotnet restore src/TransactionService.API/TransactionService.API.csproj
COPY . .
WORKDIR /src/src/TransactionService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "TransactionService.API.dll"]
