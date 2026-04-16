# ── LetTransactionService API Dockerfile ──────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5274
ENV ASPNETCORE_URLS=http://+:5274

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/LetTransactionService.API/LetTransactionService.API.csproj", "src/LetTransactionService.API/"]
COPY ["src/LetTransactionService.Application/LetTransactionService.Application.csproj", "src/LetTransactionService.Application/"]
COPY ["src/LetTransactionService.Domain/LetTransactionService.Domain.csproj", "src/LetTransactionService.Domain/"]
COPY ["src/LetTransactionService.Infrastructure/LetTransactionService.Infrastructure.csproj", "src/LetTransactionService.Infrastructure/"]
RUN dotnet restore "src/LetTransactionService.API/LetTransactionService.API.csproj"
COPY . .
WORKDIR "/src/src/LetTransactionService.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "LetTransactionService.API.dll"]
