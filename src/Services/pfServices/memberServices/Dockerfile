FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

COPY ["src/MemberService.API/MemberService.API.csproj", "src/MemberService.API/"]
COPY ["src/MemberService.Application/MemberService.Application.csproj", "src/MemberService.Application/"]
COPY ["src/MemberService.Domain/MemberService.Domain.csproj", "src/MemberService.Domain/"]
COPY ["src/MemberService.Infrastructure/MemberService.Infrastructure.csproj", "src/MemberService.Infrastructure/"]
RUN dotnet restore "src/MemberService.API/MemberService.API.csproj"

COPY . .
RUN dotnet publish "src/MemberService.API/MemberService.API.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MemberService.API.dll"]
