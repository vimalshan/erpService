FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY src/InvoiceProcessing.API/InvoiceProcessing.API.csproj src/InvoiceProcessing.API/
COPY src/InvoiceProcessing.Application/InvoiceProcessing.Application.csproj src/InvoiceProcessing.Application/
COPY src/InvoiceProcessing.Domain/InvoiceProcessing.Domain.csproj src/InvoiceProcessing.Domain/
COPY src/InvoiceProcessing.Infrastructure/InvoiceProcessing.Infrastructure.csproj src/InvoiceProcessing.Infrastructure/
RUN dotnet restore src/InvoiceProcessing.API/InvoiceProcessing.API.csproj

COPY . .
RUN dotnet publish src/InvoiceProcessing.API/InvoiceProcessing.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "InvoiceProcessing.API.dll"]
