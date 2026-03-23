FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/LeaveServices.API/LeaveServices.API.csproj LeaveServices.API/
COPY src/LeaveServices.Application/LeaveServices.Application.csproj LeaveServices.Application/
COPY src/LeaveServices.Domain/LeaveServices.Domain.csproj LeaveServices.Domain/
COPY src/LeaveServices.Infrastructure/LeaveServices.Infrastructure.csproj LeaveServices.Infrastructure/
RUN dotnet restore LeaveServices.API/LeaveServices.API.csproj

COPY src/ .
RUN find . -name 'bin' -o -name 'obj' | xargs rm -rf 2>/dev/null || true
RUN dotnet restore LeaveServices.API/LeaveServices.API.csproj
WORKDIR /src/LeaveServices.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "LeaveServices.API.dll"]
