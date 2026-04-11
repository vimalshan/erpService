FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY src/AdminService.Domain/AdminService.Domain.csproj src/AdminService.Domain/
COPY src/AdminService.Application/AdminService.Application.csproj src/AdminService.Application/
COPY src/AdminService.Infrastructure/AdminService.Infrastructure.csproj src/AdminService.Infrastructure/
COPY src/AdminService.API/AdminService.API.csproj src/AdminService.API/
RUN dotnet restore src/AdminService.API/AdminService.API.csproj
COPY . .
WORKDIR /src/src/AdminService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "AdminService.API.dll"]
