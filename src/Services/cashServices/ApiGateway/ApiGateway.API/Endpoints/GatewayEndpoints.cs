using ApiGateway.API.Configuration;
using Microsoft.Extensions.Options;

namespace ApiGateway.API.Endpoints;

public static class GatewayEndpoints
{
    public static IEndpointRouteBuilder MapGatewayEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/gateway")
            .WithTags("Gateway");

        group.MapGet("/ping", () => Results.Ok(new
        {
            service = "ApiGateway",
            status = "Running",
            timestamp = DateTime.UtcNow,
            version = "1.0.0"
        }))
        .WithName("Ping");

        group.MapGet("/routes", (IOptions<ServiceEndpoints> endpoints) =>
        {
            var ep = endpoints.Value;
            var routes = new[]
            {
                new { Service = "CashManagement", Rest = "/api/v1/cash/{**path}", GraphQl = "/graphql/cash", Health = $"{ep.CashManagement}/health" },
                new { Service = "CurrencyManagement", Rest = "/api/v1/currency/{**path}", GraphQl = "/graphql/currency", Health = $"{ep.CurrencyManagement}/health" },
                new { Service = "DealTicketing", Rest = "/api/v1/deals/{**path}", GraphQl = "/graphql/deals", Health = $"{ep.DealTicketing}/health" },
                new { Service = "LoanManagement", Rest = "/api/v1/loans/{**path}", GraphQl = "/graphql/loans", Health = $"{ep.LoanManagement}/health" },
                new { Service = "OrganizationSetup", Rest = "/api/v1/organization/{**path}", GraphQl = "/graphql/organization", Health = $"{ep.OrganizationSetup}/health" },
                new { Service = "EmailNotification", Rest = "/api/v1/email/{**path}", GraphQl = "/graphql/email", Health = $"{ep.EmailNotification}/health" },
            };
            return Results.Ok(routes);
        })
        .WithName("GetRoutes");

        group.MapGet("/config", (IConfiguration config) =>
        {
            return Results.Ok(new
            {
                rateLimiting = new
                {
                    permitLimit = config.GetValue<int>("RateLimiting:PermitLimit"),
                    windowSeconds = config.GetValue<int>("RateLimiting:WindowSeconds"),
                    queueLimit = config.GetValue<int>("RateLimiting:QueueLimit")
                },
                healthCheckInterval = "30s",
                reverseProxy = "YARP 2.3.0"
            });
        })
        .WithName("GetConfig")
        .RequireAuthorization();

        return endpoints;
    }
}
