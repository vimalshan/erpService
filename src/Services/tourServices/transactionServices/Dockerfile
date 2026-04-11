FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/TransactionService.API/TransactionService.API.csproj", "src/TransactionService.API/"]
COPY ["src/TransactionService.Application/TransactionService.Application.csproj", "src/TransactionService.Application/"]
COPY ["src/TransactionService.Domain/TransactionService.Domain.csproj", "src/TransactionService.Domain/"]
COPY ["src/TransactionService.Infrastructure/TransactionService.Infrastructure.csproj", "src/TransactionService.Infrastructure/"]
RUN dotnet restore "src/TransactionService.API/TransactionService.API.csproj"
COPY . .
WORKDIR "/src/src/TransactionService.API"
RUN dotnet build "TransactionService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TransactionService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=10s --retries=3 CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "TransactionService.API.dll"]
