FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

COPY ["src/InvestmentService.API/InvestmentService.API.csproj", "src/InvestmentService.API/"]
COPY ["src/InvestmentService.Application/InvestmentService.Application.csproj", "src/InvestmentService.Application/"]
COPY ["src/InvestmentService.Domain/InvestmentService.Domain.csproj", "src/InvestmentService.Domain/"]
COPY ["src/InvestmentService.Infrastructure/InvestmentService.Infrastructure.csproj", "src/InvestmentService.Infrastructure/"]
RUN dotnet restore "src/InvestmentService.API/InvestmentService.API.csproj"

COPY . .
RUN dotnet publish "src/InvestmentService.API/InvestmentService.API.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InvestmentService.API.dll"]
