FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5440
ENV ASPNETCORE_URLS=http://+:5440

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY src/MenuAndSecurityService.API/MenuAndSecurityService.API.csproj src/MenuAndSecurityService.API/
COPY src/MenuAndSecurityService.Application/MenuAndSecurityService.Application.csproj src/MenuAndSecurityService.Application/
COPY src/MenuAndSecurityService.Domain/MenuAndSecurityService.Domain.csproj src/MenuAndSecurityService.Domain/
COPY src/MenuAndSecurityService.Infrastructure/MenuAndSecurityService.Infrastructure.csproj src/MenuAndSecurityService.Infrastructure/
RUN dotnet restore src/MenuAndSecurityService.API/MenuAndSecurityService.API.csproj

COPY . .
RUN dotnet publish src/MenuAndSecurityService.API/MenuAndSecurityService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MenuAndSecurityService.API.dll"]
