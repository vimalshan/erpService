# ── CourseService API Dockerfile ──────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5271
ENV ASPNETCORE_URLS=http://+:5271

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/CourseService.API/CourseService.API.csproj", "src/CourseService.API/"]
COPY ["src/CourseService.Application/CourseService.Application.csproj", "src/CourseService.Application/"]
COPY ["src/CourseService.Domain/CourseService.Domain.csproj", "src/CourseService.Domain/"]
COPY ["src/CourseService.Infrastructure/CourseService.Infrastructure.csproj", "src/CourseService.Infrastructure/"]
RUN dotnet restore "src/CourseService.API/CourseService.API.csproj"
COPY . .
WORKDIR "/src/src/CourseService.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "CourseService.API.dll"]
