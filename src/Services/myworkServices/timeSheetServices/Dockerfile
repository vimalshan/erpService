FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5337
ENV ASPNETCORE_URLS=http://+:5337

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/TimeSheetService.Domain/TimeSheetService.Domain.csproj",           "src/TimeSheetService.Domain/"]
COPY ["src/TimeSheetService.Application/TimeSheetService.Application.csproj", "src/TimeSheetService.Application/"]
COPY ["src/TimeSheetService.Infrastructure/TimeSheetService.Infrastructure.csproj", "src/TimeSheetService.Infrastructure/"]
COPY ["src/TimeSheetService.API/TimeSheetService.API.csproj",                 "src/TimeSheetService.API/"]
RUN dotnet restore "src/TimeSheetService.API/TimeSheetService.API.csproj"

COPY . .
RUN dotnet publish "src/TimeSheetService.API/TimeSheetService.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TimeSheetService.API.dll"]
