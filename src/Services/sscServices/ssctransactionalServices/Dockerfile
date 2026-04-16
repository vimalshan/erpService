FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5442
ENV ASPNETCORE_URLS=http://+:5442

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY src/SSCTransactional.API/SSCTransactional.API.csproj src/SSCTransactional.API/
COPY src/SSCTransactional.Application/SSCTransactional.Application.csproj src/SSCTransactional.Application/
COPY src/SSCTransactional.Domain/SSCTransactional.Domain.csproj src/SSCTransactional.Domain/
COPY src/SSCTransactional.Infrastructure/SSCTransactional.Infrastructure.csproj src/SSCTransactional.Infrastructure/
RUN dotnet restore src/SSCTransactional.API/SSCTransactional.API.csproj

COPY . .
RUN dotnet publish src/SSCTransactional.API/SSCTransactional.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SSCTransactional.API.dll"]
