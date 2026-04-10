FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY src/BatchAndEnvelopeService.API/BatchAndEnvelopeService.API.csproj src/BatchAndEnvelopeService.API/
COPY src/BatchAndEnvelopeService.Application/BatchAndEnvelopeService.Application.csproj src/BatchAndEnvelopeService.Application/
COPY src/BatchAndEnvelopeService.Domain/BatchAndEnvelopeService.Domain.csproj src/BatchAndEnvelopeService.Domain/
COPY src/BatchAndEnvelopeService.Infrastructure/BatchAndEnvelopeService.Infrastructure.csproj src/BatchAndEnvelopeService.Infrastructure/
RUN dotnet restore src/BatchAndEnvelopeService.API/BatchAndEnvelopeService.API.csproj

COPY . .
RUN dotnet publish src/BatchAndEnvelopeService.API/BatchAndEnvelopeService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "BatchAndEnvelopeService.API.dll"]
