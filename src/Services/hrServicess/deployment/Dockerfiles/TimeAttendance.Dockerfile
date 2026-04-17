# Multi-stage build for TimeAttendance.API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["timeattendanceServices/src/TimeAttendance.API/TimeAttendance.API.csproj", "timeattendanceServices/src/TimeAttendance.API/"]
COPY ["timeattendanceServices/src/TimeAttendance.Application/TimeAttendance.Application.csproj", "timeattendanceServices/src/TimeAttendance.Application/"]
COPY ["timeattendanceServices/src/TimeAttendance.Domain/TimeAttendance.Domain.csproj", "timeattendanceServices/src/TimeAttendance.Domain/"]
COPY ["timeattendanceServices/src/TimeAttendance.Infrastructure/TimeAttendance.Infrastructure.csproj", "timeattendanceServices/src/TimeAttendance.Infrastructure/"]

RUN dotnet restore "timeattendanceServices/src/TimeAttendance.API/TimeAttendance.API.csproj"

COPY . .

RUN dotnet build "timeattendanceServices/src/TimeAttendance.API/TimeAttendance.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "timeattendanceServices/src/TimeAttendance.API/TimeAttendance.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 5235
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5235

RUN apt-get update --fix-missing && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/* || \
    (apt-get update --fix-missing && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*)

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TimeAttendance.API.dll"]
