# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
EXPOSE 5323
ENV ASPNETCORE_URLS=http://+:5323

# Copy project files for restore
COPY ["UserManagement.API/UserManagement.API.csproj", "UserManagement.API/"]
COPY ["UserManagement.Application/UserManagement.Application.csproj", "UserManagement.Application/"]
COPY ["UserManagement.Domain/UserManagement.Domain.csproj", "UserManagement.Domain/"]
COPY ["UserManagement.Infrastructure/UserManagement.Infrastructure.csproj", "UserManagement.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "UserManagement.API/UserManagement.API.csproj"

# Copy source code
COPY UserManagement.API/ UserManagement.API/
COPY UserManagement.Application/ UserManagement.Application/
COPY UserManagement.Domain/ UserManagement.Domain/
COPY UserManagement.Infrastructure/ UserManagement.Infrastructure/

# Publish
RUN dotnet publish "UserManagement.API/UserManagement.API.csproj" -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /publish .

ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD wget -qO- http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "UserManagement.API.dll"]
