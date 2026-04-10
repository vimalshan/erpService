FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY src/ClubMembershipService.API/ClubMembershipService.API.csproj src/ClubMembershipService.API/
COPY src/ClubMembershipService.Application/ClubMembershipService.Application.csproj src/ClubMembershipService.Application/
COPY src/ClubMembershipService.Domain/ClubMembershipService.Domain.csproj src/ClubMembershipService.Domain/
COPY src/ClubMembershipService.Infrastructure/ClubMembershipService.Infrastructure.csproj src/ClubMembershipService.Infrastructure/
RUN dotnet restore src/ClubMembershipService.API/ClubMembershipService.API.csproj

COPY . .
RUN dotnet publish src/ClubMembershipService.API/ClubMembershipService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ClubMembershipService.API.dll"]
