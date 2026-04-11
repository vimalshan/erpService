FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/AdminService.API/AdminService.API.csproj", "src/AdminService.API/"]
COPY ["src/AdminService.Application/AdminService.Application.csproj", "src/AdminService.Application/"]
COPY ["src/AdminService.Domain/AdminService.Domain.csproj", "src/AdminService.Domain/"]
COPY ["src/AdminService.Infrastructure/AdminService.Infrastructure.csproj", "src/AdminService.Infrastructure/"]
RUN dotnet restore "src/AdminService.API/AdminService.API.csproj"
COPY . .
WORKDIR "/src/src/AdminService.API"
RUN dotnet build "AdminService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdminService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=10s --retries=3 CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "AdminService.API.dll"]
