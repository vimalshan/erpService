FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/HRDocumentService.API/HRDocumentService.API.csproj src/HRDocumentService.API/
COPY src/HRDocumentService.Application/HRDocumentService.Application.csproj src/HRDocumentService.Application/
COPY src/HRDocumentService.Domain/HRDocumentService.Domain.csproj src/HRDocumentService.Domain/
COPY src/HRDocumentService.Infrastructure/HRDocumentService.Infrastructure.csproj src/HRDocumentService.Infrastructure/
RUN dotnet restore src/HRDocumentService.API/HRDocumentService.API.csproj

COPY . .
RUN dotnet publish src/HRDocumentService.API/HRDocumentService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "HRDocumentService.API.dll"]
