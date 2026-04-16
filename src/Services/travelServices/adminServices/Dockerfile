FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["AdminService.API/AdminService.API.csproj", "AdminService.API/"]
COPY ["AdminService.Application/AdminService.Application.csproj", "AdminService.Application/"]
COPY ["AdminService.Domain/AdminService.Domain.csproj", "AdminService.Domain/"]
COPY ["AdminService.Infrastructure/AdminService.Infrastructure.csproj", "AdminService.Infrastructure/"]
COPY ["AdminService.Shared/AdminService.Shared.csproj", "AdminService.Shared/"]

RUN dotnet restore "AdminService.API/AdminService.API.csproj"

COPY . .

RUN dotnet publish "AdminService.API/AdminService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 5491
ENV ASPNETCORE_URLS=http://+:5491

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AdminService.API.dll"]
