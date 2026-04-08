FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["FillingOperationService/src/FillingOperationService.API/FillingOperationService.API.csproj", "FillingOperationService/src/FillingOperationService.API/"]
COPY ["FillingOperationService/src/FillingOperationService.Application/FillingOperationService.Application.csproj", "FillingOperationService/src/FillingOperationService.Application/"]
COPY ["FillingOperationService/src/FillingOperationService.Domain/FillingOperationService.Domain.csproj", "FillingOperationService/src/FillingOperationService.Domain/"]
COPY ["FillingOperationService/src/FillingOperationService.Infrastructure/FillingOperationService.Infrastructure.csproj", "FillingOperationService/src/FillingOperationService.Infrastructure/"]
COPY ["FillingOperationService/src/FillingOperationService.Functions/FillingOperationService.Functions.csproj", "FillingOperationService/src/FillingOperationService.Functions/"]
RUN dotnet restore "FillingOperationService/src/FillingOperationService.API/FillingOperationService.API.csproj"
COPY . .
WORKDIR "/src/FillingOperationService/src/FillingOperationService.API"
RUN dotnet build "FillingOperationService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FillingOperationService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "FillingOperationService.API.dll"]
