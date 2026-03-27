using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Functions.Functions;

public sealed class ServiceDiscoveryFunction
{
    private readonly ILogger<ServiceDiscoveryFunction> _logger;

    private static readonly List<ServiceRegistration> RegisteredServices =
    [
        new("CashManagement", "http://localhost:5249", "/api/v1/cash", "/graphql/cash",
            ["CashTransactions", "BankAccounts", "BankTransactions", "Cheques", "Reconciliation", "CashUnits"]),
        new("CurrencyManagement", "http://localhost:5031", "/api/v1/currency", "/graphql/currency",
            ["Currencies", "ExchangeRates", "OrgCurrencyMap"]),
        new("DealTicketing", "http://localhost:5081", "/api/v1/deals", "/graphql/deals",
            ["DealBatches", "DealDetails", "Settlements", "DealAttachments"]),
        new("LoanManagement", "http://localhost:5268", "/api/v1/loans", "/graphql/loans",
            ["Loans", "Disbursements", "Interest", "Repayments"]),
        new("OrganizationSetup", "http://localhost:5099", "/api/v1/organization", "/graphql/organization",
            ["Roles", "UserMapping", "OrgParams", "PPLimits"]),
        new("EmailNotification", "http://localhost:5032", "/api/v1/email", "/graphql/email",
            ["EmailTypes", "MailAccess"]),
    ];

    public ServiceDiscoveryFunction(ILogger<ServiceDiscoveryFunction> logger)
    {
        _logger = logger;
    }

    [Function("ServiceDiscovery")]
    public async Task<HttpResponseData> GetServices(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "services")] HttpRequestData req)
    {
        _logger.LogInformation("Service discovery request at {Time}", DateTime.UtcNow);

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new
        {
            gateway = new { url = "http://localhost:5000", graphql = "/graphql", health = "/health" },
            services = RegisteredServices,
            registeredAt = DateTime.UtcNow
        });
        return response;
    }

    [Function("ServiceLookup")]
    public async Task<HttpResponseData> LookupService(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "services/{serviceName}")] HttpRequestData req,
        string serviceName)
    {
        var service = RegisteredServices.Find(s =>
            s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase));

        if (service is null)
        {
            var response = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
            await response.WriteAsJsonAsync(new { message = $"Service '{serviceName}' not found." });
            return response;
        }

        var okResponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await okResponse.WriteAsJsonAsync(service);
        return okResponse;
    }
}

public sealed record ServiceRegistration(
    string Name,
    string BaseUrl,
    string RestPrefix,
    string GraphQlPrefix,
    List<string> Resources);
