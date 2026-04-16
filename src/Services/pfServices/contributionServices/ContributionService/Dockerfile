FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5373
ENV ASPNETCORE_URLS=http://+:5373
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

COPY ["src/ContributionService.API/ContributionService.API.csproj", "src/ContributionService.API/"]
COPY ["src/ContributionService.Application/ContributionService.Application.csproj", "src/ContributionService.Application/"]
COPY ["src/ContributionService.Domain/ContributionService.Domain.csproj", "src/ContributionService.Domain/"]
COPY ["src/ContributionService.Infrastructure/ContributionService.Infrastructure.csproj", "src/ContributionService.Infrastructure/"]
RUN dotnet restore "src/ContributionService.API/ContributionService.API.csproj"

COPY . .
RUN dotnet publish "src/ContributionService.API/ContributionService.API.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ContributionService.API.dll"]
