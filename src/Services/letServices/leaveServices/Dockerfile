# ── LeaveServices API Dockerfile ──────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5273
ENV ASPNETCORE_URLS=http://+:5273

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/LeaveServices.API/LeaveServices.API.csproj", "src/LeaveServices.API/"]
COPY ["src/LeaveServices.Application/LeaveServices.Application.csproj", "src/LeaveServices.Application/"]
COPY ["src/LeaveServices.Domain/LeaveServices.Domain.csproj", "src/LeaveServices.Domain/"]
COPY ["src/LeaveServices.Infrastructure/LeaveServices.Infrastructure.csproj", "src/LeaveServices.Infrastructure/"]
RUN dotnet restore "src/LeaveServices.API/LeaveServices.API.csproj"
COPY . .
WORKDIR "/src/src/LeaveServices.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "LeaveServices.API.dll"]
