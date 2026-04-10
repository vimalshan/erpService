FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/CategoryAndVendorService.API/CategoryAndVendorService.API.csproj src/CategoryAndVendorService.API/
COPY src/CategoryAndVendorService.Application/CategoryAndVendorService.Application.csproj src/CategoryAndVendorService.Application/
COPY src/CategoryAndVendorService.Domain/CategoryAndVendorService.Domain.csproj src/CategoryAndVendorService.Domain/
COPY src/CategoryAndVendorService.Infrastructure/CategoryAndVendorService.Infrastructure.csproj src/CategoryAndVendorService.Infrastructure/
RUN dotnet restore src/CategoryAndVendorService.API/CategoryAndVendorService.API.csproj

COPY . .
RUN dotnet publish src/CategoryAndVendorService.API/CategoryAndVendorService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "CategoryAndVendorService.API.dll"]
