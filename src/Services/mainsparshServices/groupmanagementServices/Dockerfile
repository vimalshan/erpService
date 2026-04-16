# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
EXPOSE 5315
ENV ASPNETCORE_URLS=http://+:5315

# Copy project files for restore
COPY ["GroupManagementModule/GroupManagementService.API/GroupManagementService.API.csproj", "GroupManagementModule/GroupManagementService.API/"]
COPY ["GroupManagementModule/GroupManagementService.Application/GroupManagementService.Application.csproj", "GroupManagementModule/GroupManagementService.Application/"]
COPY ["GroupManagementModule/GroupManagementService.Domain/GroupManagementService.Domain.csproj", "GroupManagementModule/GroupManagementService.Domain/"]
COPY ["GroupManagementModule/GroupManagementService.Infrastructure/GroupManagementService.Infrastructure.csproj", "GroupManagementModule/GroupManagementService.Infrastructure/"]
COPY ["GroupManagementModule/GroupManagementService.BackgroundTasks/GroupManagementService.BackgroundTasks.csproj", "GroupManagementModule/GroupManagementService.BackgroundTasks/"]

# Restore dependencies
RUN dotnet restore "GroupManagementModule/GroupManagementService.API/GroupManagementService.API.csproj"

# Copy source code
COPY GroupManagementModule/GroupManagementService.API/ GroupManagementModule/GroupManagementService.API/
COPY GroupManagementModule/GroupManagementService.Application/ GroupManagementModule/GroupManagementService.Application/
COPY GroupManagementModule/GroupManagementService.Domain/ GroupManagementModule/GroupManagementService.Domain/
COPY GroupManagementModule/GroupManagementService.Infrastructure/ GroupManagementModule/GroupManagementService.Infrastructure/
COPY GroupManagementModule/GroupManagementService.BackgroundTasks/ GroupManagementModule/GroupManagementService.BackgroundTasks/

# Publish
RUN dotnet publish "GroupManagementModule/GroupManagementService.API/GroupManagementService.API.csproj" -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
COPY --from=build /publish .

ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "GroupManagementService.API.dll"]
