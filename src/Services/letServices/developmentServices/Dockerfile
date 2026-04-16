# ── DevelopmentService API Dockerfile ─────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5272
ENV ASPNETCORE_URLS=http://+:5272

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/DevelopmentService.API/DevelopmentService.API.csproj", "src/DevelopmentService.API/"]
COPY ["src/DevelopmentService.Application/DevelopmentService.Application.csproj", "src/DevelopmentService.Application/"]
COPY ["src/DevelopmentService.Domain/DevelopmentService.Domain.csproj", "src/DevelopmentService.Domain/"]
COPY ["src/DevelopmentService.Infrastructure/DevelopmentService.Infrastructure.csproj", "src/DevelopmentService.Infrastructure/"]
RUN dotnet restore "src/DevelopmentService.API/DevelopmentService.API.csproj"
COPY . .
WORKDIR "/src/src/DevelopmentService.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "DevelopmentService.API.dll"]
