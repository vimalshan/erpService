FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY src/FilingAndArchiveService.API/FilingAndArchiveService.API.csproj src/FilingAndArchiveService.API/
COPY src/FilingAndArchiveService.Application/FilingAndArchiveService.Application.csproj src/FilingAndArchiveService.Application/
COPY src/FilingAndArchiveService.Domain/FilingAndArchiveService.Domain.csproj src/FilingAndArchiveService.Domain/
COPY src/FilingAndArchiveService.Infrastructure/FilingAndArchiveService.Infrastructure.csproj src/FilingAndArchiveService.Infrastructure/
RUN dotnet restore src/FilingAndArchiveService.API/FilingAndArchiveService.API.csproj

COPY . .
RUN dotnet publish src/FilingAndArchiveService.API/FilingAndArchiveService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "FilingAndArchiveService.API.dll"]
