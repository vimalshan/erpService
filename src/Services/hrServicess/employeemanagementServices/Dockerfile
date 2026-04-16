# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/EmployeeManagement.API/EmployeeManagement.API.csproj src/EmployeeManagement.API/

RUN dotnet restore src/EmployeeManagement.API/EmployeeManagement.API.csproj

COPY . .
RUN dotnet publish src/EmployeeManagement.API/EmployeeManagement.API.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5253
ENV ASPNETCORE_URLS=http://+:5253

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
  CMD wget -qO- http://localhost:5253/health/live || exit 1

ENTRYPOINT ["dotnet", "EmployeeManagement.API.dll"]
