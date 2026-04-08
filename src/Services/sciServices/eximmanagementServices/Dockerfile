FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["src/EximManagement.API/EximManagement.API.csproj", "src/EximManagement.API/"]
COPY ["src/EximManagement.Application/EximManagement.Application.csproj", "src/EximManagement.Application/"]
COPY ["src/EximManagement.Domain/EximManagement.Domain.csproj", "src/EximManagement.Domain/"]
COPY ["src/EximManagement.Infrastructure/EximManagement.Infrastructure.csproj", "src/EximManagement.Infrastructure/"]
COPY ["src/EximManagement.Functions/EximManagement.Functions.csproj", "src/EximManagement.Functions/"]
RUN dotnet restore "src/EximManagement.API/EximManagement.API.csproj"
COPY . .
WORKDIR "/src/src/EximManagement.API"
RUN dotnet build "EximManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EximManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "EximManagement.API.dll"]
