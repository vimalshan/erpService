FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["BookingService/src/BookingService.API/BookingService.API.csproj", "BookingService/src/BookingService.API/"]
COPY ["BookingService/src/BookingService.Application/BookingService.Application.csproj", "BookingService/src/BookingService.Application/"]
COPY ["BookingService/src/BookingService.Domain/BookingService.Domain.csproj", "BookingService/src/BookingService.Domain/"]
COPY ["BookingService/src/BookingService.Infrastructure/BookingService.Infrastructure.csproj", "BookingService/src/BookingService.Infrastructure/"]

RUN dotnet restore "BookingService/src/BookingService.API/BookingService.API.csproj"

COPY . .

RUN dotnet publish "BookingService/src/BookingService.API/BookingService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookingService.API.dll"]
