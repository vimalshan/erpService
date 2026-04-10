FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MobileExpenseManagement.API/MobileExpenseManagement.API.csproj", "MobileExpenseManagement.API/"]
COPY ["MobileExpenseManagement.Application/MobileExpenseManagement.Application.csproj", "MobileExpenseManagement.Application/"]
COPY ["MobileExpenseManagement.Domain/MobileExpenseManagement.Domain.csproj", "MobileExpenseManagement.Domain/"]
COPY ["MobileExpenseManagement.Infrastructure/MobileExpenseManagement.Infrastructure.csproj", "MobileExpenseManagement.Infrastructure/"]
RUN dotnet restore "MobileExpenseManagement.API/MobileExpenseManagement.API.csproj"
COPY . .
WORKDIR "/src/MobileExpenseManagement.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "MobileExpenseManagement.API.dll"]
