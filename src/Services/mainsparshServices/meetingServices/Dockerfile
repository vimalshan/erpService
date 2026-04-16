# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
EXPOSE 5317
ENV ASPNETCORE_URLS=http://+:5317

# Copy project files for restore
COPY ["MeetingModule/src/MeetingModule.API/MeetingModule.API.csproj", "MeetingModule/src/MeetingModule.API/"]
COPY ["MeetingModule/src/MeetingModule.Application/MeetingModule.Application.csproj", "MeetingModule/src/MeetingModule.Application/"]
COPY ["MeetingModule/src/MeetingModule.Domain/MeetingModule.Domain.csproj", "MeetingModule/src/MeetingModule.Domain/"]
COPY ["MeetingModule/src/MeetingModule.Infrastructure/MeetingModule.Infrastructure.csproj", "MeetingModule/src/MeetingModule.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "MeetingModule/src/MeetingModule.API/MeetingModule.API.csproj"

# Copy source code
COPY MeetingModule/src/MeetingModule.API/ MeetingModule/src/MeetingModule.API/
COPY MeetingModule/src/MeetingModule.Application/ MeetingModule/src/MeetingModule.Application/
COPY MeetingModule/src/MeetingModule.Domain/ MeetingModule/src/MeetingModule.Domain/
COPY MeetingModule/src/MeetingModule.Infrastructure/ MeetingModule/src/MeetingModule.Infrastructure/

# Publish
RUN dotnet publish "MeetingModule/src/MeetingModule.API/MeetingModule.API.csproj" -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /publish .

ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD wget -qO- http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "MeetingModule.API.dll"]
