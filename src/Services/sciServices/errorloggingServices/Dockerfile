FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["ErrorLoggingService/src/ErrorLoggingService.API/ErrorLoggingService.API.csproj", "ErrorLoggingService/src/ErrorLoggingService.API/"]
COPY ["ErrorLoggingService/src/ErrorLoggingService.Application/ErrorLoggingService.Application.csproj", "ErrorLoggingService/src/ErrorLoggingService.Application/"]
COPY ["ErrorLoggingService/src/ErrorLoggingService.Domain/ErrorLoggingService.Domain.csproj", "ErrorLoggingService/src/ErrorLoggingService.Domain/"]
COPY ["ErrorLoggingService/src/ErrorLoggingService.Infrastructure/ErrorLoggingService.Infrastructure.csproj", "ErrorLoggingService/src/ErrorLoggingService.Infrastructure/"]
RUN dotnet restore "ErrorLoggingService/src/ErrorLoggingService.API/ErrorLoggingService.API.csproj"
COPY . .
WORKDIR "/src/ErrorLoggingService/src/ErrorLoggingService.API"
RUN dotnet build "ErrorLoggingService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ErrorLoggingService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "ErrorLoggingService.API.dll"]
