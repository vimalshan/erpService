FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/BookingService.API/BookingService.API.csproj", "src/BookingService.API/"]
COPY ["src/BookingService.Application/BookingService.Application.csproj", "src/BookingService.Application/"]
COPY ["src/BookingService.Domain/BookingService.Domain.csproj", "src/BookingService.Domain/"]
COPY ["src/BookingService.Infrastructure/BookingService.Infrastructure.csproj", "src/BookingService.Infrastructure/"]
COPY ["src/BookingService.Functions/BookingService.Functions.csproj", "src/BookingService.Functions/"]
RUN dotnet restore "src/BookingService.API/BookingService.API.csproj"
COPY . .
WORKDIR "/src/src/BookingService.API"
RUN dotnet build "BookingService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookingService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=10s --retries=3 CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "BookingService.API.dll"]
