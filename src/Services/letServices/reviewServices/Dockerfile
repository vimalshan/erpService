# ── ReviewService API Dockerfile ──────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5277
ENV ASPNETCORE_URLS=http://+:5277

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/ReviewService.API/ReviewService.API.csproj", "src/ReviewService.API/"]
COPY ["src/ReviewService.Application/ReviewService.Application.csproj", "src/ReviewService.Application/"]
COPY ["src/ReviewService.Domain/ReviewService.Domain.csproj", "src/ReviewService.Domain/"]
COPY ["src/ReviewService.Infrastructure/ReviewService.Infrastructure.csproj", "src/ReviewService.Infrastructure/"]
RUN dotnet restore "src/ReviewService.API/ReviewService.API.csproj"
COPY . .
WORKDIR "/src/src/ReviewService.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "ReviewService.API.dll"]
