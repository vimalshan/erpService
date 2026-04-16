FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5127
ENV ASPNETCORE_URLS=http://+:5127

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/GroupIncentiveService.API/GroupIncentiveService.API.csproj GroupIncentiveService.API/
COPY src/GroupIncentiveService.Application/GroupIncentiveService.Application.csproj GroupIncentiveService.Application/
COPY src/GroupIncentiveService.Domain/GroupIncentiveService.Domain.csproj GroupIncentiveService.Domain/
COPY src/GroupIncentiveService.Infrastructure/GroupIncentiveService.Infrastructure.csproj GroupIncentiveService.Infrastructure/
RUN dotnet restore GroupIncentiveService.API/GroupIncentiveService.API.csproj

COPY src/ .
RUN find . -name 'bin' -o -name 'obj' | xargs rm -rf 2>/dev/null || true
RUN dotnet restore GroupIncentiveService.API/GroupIncentiveService.API.csproj
WORKDIR /src/GroupIncentiveService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "GroupIncentiveService.API.dll"]
