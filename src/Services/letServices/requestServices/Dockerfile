# ── RequestServices API Dockerfile ────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/RequestServices.API/RequestServices.API.csproj", "src/RequestServices.API/"]
COPY ["src/RequestServices.Application/RequestServices.Application.csproj", "src/RequestServices.Application/"]
COPY ["src/RequestServices.Domain/RequestServices.Domain.csproj", "src/RequestServices.Domain/"]
COPY ["src/RequestServices.Infrastructure/RequestServices.Infrastructure.csproj", "src/RequestServices.Infrastructure/"]
RUN dotnet restore "src/RequestServices.API/RequestServices.API.csproj"
COPY . .
WORKDIR "/src/src/RequestServices.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "RequestServices.API.dll"]
