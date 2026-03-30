# Multi-stage build for UserSecurityService.API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["usersecurityServices/src/UserSecurityService.API/UserSecurityService.API.csproj", "usersecurityServices/src/UserSecurityService.API/"]
COPY ["usersecurityServices/src/UserSecurityService.Application/UserSecurityService.Application.csproj", "usersecurityServices/src/UserSecurityService.Application/"]
COPY ["usersecurityServices/src/UserSecurityService.Domain/UserSecurityService.Domain.csproj", "usersecurityServices/src/UserSecurityService.Domain/"]
COPY ["usersecurityServices/src/UserSecurityService.Infrastructure/UserSecurityService.Infrastructure.csproj", "usersecurityServices/src/UserSecurityService.Infrastructure/"]

RUN dotnet restore "usersecurityServices/src/UserSecurityService.API/UserSecurityService.API.csproj"

COPY . .

RUN dotnet build "usersecurityServices/src/UserSecurityService.API/UserSecurityService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "usersecurityServices/src/UserSecurityService.API/UserSecurityService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5140
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5140

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "UserSecurityService.API.dll"]
