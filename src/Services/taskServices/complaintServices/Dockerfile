FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5451
ENV ASPNETCORE_URLS=http://+:5451

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ComplaintService/src/ComplaintService.Domain/ComplaintService.Domain.csproj ComplaintService/src/ComplaintService.Domain/
COPY ComplaintService/src/ComplaintService.Application/ComplaintService.Application.csproj ComplaintService/src/ComplaintService.Application/
COPY ComplaintService/src/ComplaintService.Infrastructure/ComplaintService.Infrastructure.csproj ComplaintService/src/ComplaintService.Infrastructure/
COPY ComplaintService/src/ComplaintService.API/ComplaintService.API.csproj ComplaintService/src/ComplaintService.API/
COPY ComplaintService/src/ComplaintService.Functions/ComplaintService.Functions.csproj ComplaintService/src/ComplaintService.Functions/
RUN dotnet restore ComplaintService/src/ComplaintService.API/ComplaintService.API.csproj
COPY . .
WORKDIR /src/ComplaintService/src/ComplaintService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "ComplaintService.API.dll"]
