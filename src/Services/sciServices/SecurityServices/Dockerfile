FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5403
ENV ASPNETCORE_URLS=http://+:5403

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/SecurityService.API/SecurityService.API.csproj", "src/SecurityService.API/"]
COPY ["src/SecurityService.Application/SecurityService.Application.csproj", "src/SecurityService.Application/"]
COPY ["src/SecurityService.Domain/SecurityService.Domain.csproj", "src/SecurityService.Domain/"]
COPY ["src/SecurityService.Infrastructure/SecurityService.Infrastructure.csproj", "src/SecurityService.Infrastructure/"]
COPY ["src/SecurityService.Functions/SecurityService.Functions.csproj", "src/SecurityService.Functions/"]
RUN dotnet restore "src/SecurityService.API/SecurityService.API.csproj"
COPY . .
WORKDIR "/src/src/SecurityService.API"
RUN dotnet build "SecurityService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SecurityService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "SecurityService.API.dll"]
