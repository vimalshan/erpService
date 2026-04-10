FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/MobileAppManagement.API/MobileAppManagement.API.csproj", "src/MobileAppManagement.API/"]
COPY ["src/MobileAppManagement.Application/MobileAppManagement.Application.csproj", "src/MobileAppManagement.Application/"]
COPY ["src/MobileAppManagement.Domain/MobileAppManagement.Domain.csproj", "src/MobileAppManagement.Domain/"]
COPY ["src/MobileAppManagement.Infrastructure/MobileAppManagement.Infrastructure.csproj", "src/MobileAppManagement.Infrastructure/"]
RUN dotnet restore "src/MobileAppManagement.API/MobileAppManagement.API.csproj"
COPY . .
WORKDIR "/src/src/MobileAppManagement.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "MobileAppManagement.API.dll"]
