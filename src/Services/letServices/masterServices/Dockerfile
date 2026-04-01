# ── MasterService API Dockerfile ──────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/MasterService.API/MasterService.API.csproj", "src/MasterService.API/"]
COPY ["src/MasterService.Application/MasterService.Application.csproj", "src/MasterService.Application/"]
COPY ["src/MasterService.Domain/MasterService.Domain.csproj", "src/MasterService.Domain/"]
COPY ["src/MasterService.Infrastructure/MasterService.Infrastructure.csproj", "src/MasterService.Infrastructure/"]
RUN dotnet restore "src/MasterService.API/MasterService.API.csproj"
COPY . .
WORKDIR "/src/src/MasterService.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "MasterService.API.dll"]
