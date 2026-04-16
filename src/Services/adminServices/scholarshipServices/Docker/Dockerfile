# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /app
EXPOSE 5104
ENV ASPNETCORE_URLS=http://+:5104

# Copy solution file
COPY ScholarshipService.slnx ./

# Copy each project file individually to maximise Docker layer cache on restore
COPY src/ScholarshipService.Domain/ScholarshipService.Domain.csproj \
     ./src/ScholarshipService.Domain/
COPY src/ScholarshipService.Application/ScholarshipService.Application.csproj \
     ./src/ScholarshipService.Application/
COPY src/ScholarshipService.Infrastructure/ScholarshipService.Infrastructure.csproj \
     ./src/ScholarshipService.Infrastructure/
COPY src/ScholarshipService.API/ScholarshipService.API.csproj \
     ./src/ScholarshipService.API/

# Restore all projects
RUN dotnet restore ScholarshipService.slnx

# Copy remaining source code
COPY . .

# Build the API project in Release mode
WORKDIR /app/src/ScholarshipService.API
RUN dotnet build ScholarshipService.API.csproj -c Release -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM builder AS publish
WORKDIR /app/src/ScholarshipService.API
RUN dotnet publish ScholarshipService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# Install curl for health checks
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Health check against the /health endpoint
HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5166/health || exit 1

# Environment
ENV ASPNETCORE_ENVIRONMENT=Production

# Entry point
ENTRYPOINT ["dotnet", "ScholarshipService.API.dll"]
