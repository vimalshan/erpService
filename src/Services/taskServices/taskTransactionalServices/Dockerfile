FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5455
ENV ASPNETCORE_URLS=http://+:5455

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY src/TaskTransactional.Domain/TaskTransactional.Domain.csproj src/TaskTransactional.Domain/
COPY src/TaskTransactional.Application/TaskTransactional.Application.csproj src/TaskTransactional.Application/
COPY src/TaskTransactional.Infrastructure/TaskTransactional.Infrastructure.csproj src/TaskTransactional.Infrastructure/
COPY src/TaskTransactional.API/TaskTransactional.API.csproj src/TaskTransactional.API/
COPY src/TaskTransactional.Functions/TaskTransactional.Functions.csproj src/TaskTransactional.Functions/
RUN dotnet restore src/TaskTransactional.API/TaskTransactional.API.csproj
COPY . .
WORKDIR /src/src/TaskTransactional.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "TaskTransactional.API.dll"]
