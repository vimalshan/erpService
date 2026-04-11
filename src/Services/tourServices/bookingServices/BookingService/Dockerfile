FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY src/BookingService.Domain/BookingService.Domain.csproj src/BookingService.Domain/
COPY src/BookingService.Application/BookingService.Application.csproj src/BookingService.Application/
COPY src/BookingService.Infrastructure/BookingService.Infrastructure.csproj src/BookingService.Infrastructure/
COPY src/BookingService.Functions/BookingService.Functions.csproj src/BookingService.Functions/
COPY src/BookingService.API/BookingService.API.csproj src/BookingService.API/
RUN dotnet restore src/BookingService.API/BookingService.API.csproj
COPY . .
WORKDIR /src/src/BookingService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "BookingService.API.dll"]
