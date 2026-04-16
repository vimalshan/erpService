FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5454
ENV ASPNETCORE_URLS=http://+:5454

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY src/TaskServices.Domain/TaskServices.Domain.csproj src/TaskServices.Domain/
COPY src/TaskServices.Application/TaskServices.Application.csproj src/TaskServices.Application/
COPY src/TaskServices.Infrastructure/TaskServices.Infrastructure.csproj src/TaskServices.Infrastructure/
COPY src/TaskServices.API/TaskServices.API.csproj src/TaskServices.API/
COPY src/TaskServices.Functions/TaskServices.Functions.csproj src/TaskServices.Functions/
RUN dotnet restore src/TaskServices.API/TaskServices.API.csproj
COPY . .
WORKDIR /src/src/TaskServices.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "TaskServices.API.dll"]
