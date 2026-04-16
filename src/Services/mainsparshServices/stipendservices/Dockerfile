# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
EXPOSE 5320
ENV ASPNETCORE_URLS=http://+:5320

# Copy project files for restore
COPY ["StipendService/src/StipendService.API/StipendService.API.csproj", "StipendService/src/StipendService.API/"]
COPY ["StipendService/src/StipendService.Application/StipendService.Application.csproj", "StipendService/src/StipendService.Application/"]
COPY ["StipendService/src/StipendService.Domain/StipendService.Domain.csproj", "StipendService/src/StipendService.Domain/"]
COPY ["StipendService/src/StipendService.Infrastructure/StipendService.Infrastructure.csproj", "StipendService/src/StipendService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "StipendService/src/StipendService.API/StipendService.API.csproj"

# Copy source code
COPY StipendService/src/StipendService.API/ StipendService/src/StipendService.API/
COPY StipendService/src/StipendService.Application/ StipendService/src/StipendService.Application/
COPY StipendService/src/StipendService.Domain/ StipendService/src/StipendService.Domain/
COPY StipendService/src/StipendService.Infrastructure/ StipendService/src/StipendService.Infrastructure/

# Publish
RUN dotnet publish "StipendService/src/StipendService.API/StipendService.API.csproj" -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /publish .

ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD wget -qO- http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "StipendService.API.dll"]
