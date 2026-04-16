# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY DocumentService.slnx* ./
COPY src/DocumentService.API/DocumentService.API.csproj                         src/DocumentService.API/
COPY src/DocumentService.Application/DocumentService.Application.csproj         src/DocumentService.Application/
COPY src/DocumentService.Domain/DocumentService.Domain.csproj                   src/DocumentService.Domain/
COPY src/DocumentService.Infrastructure/DocumentService.Infrastructure.csproj   src/DocumentService.Infrastructure/

RUN dotnet restore src/DocumentService.API/DocumentService.API.csproj

COPY . .
RUN dotnet publish src/DocumentService.API/DocumentService.API.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5291
ENV ASPNETCORE_URLS=http://+:5291

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production


HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "DocumentService.API.dll"]
