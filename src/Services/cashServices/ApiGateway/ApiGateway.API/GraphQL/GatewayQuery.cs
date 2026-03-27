using ApiGateway.API.Configuration;
using Microsoft.Extensions.Options;

namespace ApiGateway.API.GraphQL;

public sealed class GatewayQuery
{
    public ServiceStatusResult GetServiceStatus(
        [Service] IOptions<ServiceEndpoints> endpoints)
    {
        var ep = endpoints.Value;
        return new ServiceStatusResult
        {
            Services =
            [
                new ServiceInfo("CashManagement", ep.CashManagement, "/api/v1/cash", "/graphql/cash"),
                new ServiceInfo("CurrencyManagement", ep.CurrencyManagement, "/api/v1/currency", "/graphql/currency"),
                new ServiceInfo("DealTicketing", ep.DealTicketing, "/api/v1/deals", "/graphql/deals"),
                new ServiceInfo("LoanManagement", ep.LoanManagement, "/api/v1/loans", "/graphql/loans"),
                new ServiceInfo("OrganizationSetup", ep.OrganizationSetup, "/api/v1/organization", "/graphql/organization"),
                new ServiceInfo("EmailNotification", ep.EmailNotification, "/api/v1/email", "/graphql/email")
            ]
        };
    }

    public string GetGatewayVersion() => "1.0.0";
}

public sealed class ServiceStatusResult
{
    public List<ServiceInfo> Services { get; set; } = [];
}

public sealed record ServiceInfo(
    string Name,
    string BaseUrl,
    string RestPrefix,
    string GraphQlPrefix);
