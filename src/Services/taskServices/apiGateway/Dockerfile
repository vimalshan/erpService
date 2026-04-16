FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5450
ENV ASPNETCORE_URLS=http://+:5450

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY src/ApiGateway/ApiGateway.csproj src/ApiGateway/
RUN dotnet restore src/ApiGateway/ApiGateway.csproj
COPY . .
WORKDIR /src/src/ApiGateway
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
