FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5520
ENV ASPNETCORE_URLS=http://+:5520
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ReceivingService.API/ReceivingService.API.csproj", "src/ReceivingService.API/"]
COPY ["src/ReceivingService.Application/ReceivingService.Application.csproj", "src/ReceivingService.Application/"]
COPY ["src/ReceivingService.Domain/ReceivingService.Domain.csproj", "src/ReceivingService.Domain/"]
COPY ["src/ReceivingService.Infrastructure/ReceivingService.Infrastructure.csproj", "src/ReceivingService.Infrastructure/"]
RUN dotnet restore "src/ReceivingService.API/ReceivingService.API.csproj"
COPY ["src/", "src/"]
RUN dotnet publish "src/ReceivingService.API/ReceivingService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ReceivingService.API.dll"]
