namespace ApiGateway.GraphQL;

public static class GraphQLExtensions
{
    public static IServiceCollection AddGatewayGraphQL(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<GraphQLSchemaProxy>();

        services
            .AddGraphQLServer()
            .AddQueryType<GatewayQuery>()
            .AddMutationType<GatewayMutation>();

        return services;
    }

    public static WebApplication MapGatewayGraphQL(this WebApplication app)
    {
        app.MapGraphQL("/graphql");
        return app;
    }
}

/// <summary>
/// Proxies GraphQL queries to downstream services.
/// </summary>
public class GraphQLSchemaProxy
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GraphQLSchemaProxy> _logger;

    private static readonly Dictionary<string, int> ServicePorts = new()
    {
        ["access"] = 5010,
        ["attendance"] = 5011,
        ["bus"] = 5012,
        ["calendar"] = 5013,
        ["employee"] = 5014,
        ["groupincentive"] = 5015,
        ["leave"] = 5016,
        ["reference"] = 5017,
        ["visitor"] = 5018
    };

    public GraphQLSchemaProxy(IHttpClientFactory httpClientFactory, ILogger<GraphQLSchemaProxy> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<string> ForwardQueryAsync(string serviceName, string query, string? variables = null)
    {
        if (!ServicePorts.TryGetValue(serviceName.ToLowerInvariant(), out var port))
            throw new ArgumentException($"Unknown service: {serviceName}");

        var client = _httpClientFactory.CreateClient($"{serviceName}-service");
        var requestBody = new { query, variables };
        var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"http://localhost:{port}/graphql", content);
        var result = await response.Content.ReadAsStringAsync();

        _logger.LogInformation("GraphQL proxy to {Service}: {StatusCode}", serviceName, response.StatusCode);
        return result;
    }
}

public class GatewayQuery
{
    public async Task<string> GetServiceData(
        [Service] GraphQLSchemaProxy proxy,
        string service,
        string query,
        string? variables = null)
    {
        return await proxy.ForwardQueryAsync(service, query, variables);
    }

    public ServiceInfo[] GetServices()
    {
        return
        [
            new("access-service", "http://localhost:5010", "Access & Security Management"),
            new("attendance-service", "http://localhost:5011", "Attendance Tracking"),
            new("bus-service", "http://localhost:5012", "Bus Transport Management"),
            new("calendar-service", "http://localhost:5013", "Calendar & Events"),
            new("employee-service", "http://localhost:5014", "Employee Management"),
            new("groupincentive-service", "http://localhost:5015", "Group Incentive Management"),
            new("leave-service", "http://localhost:5016", "Leave Management"),
            new("reference-service", "http://localhost:5017", "Reference Data"),
            new("visitor-service", "http://localhost:5018", "Visitor Management")
        ];
    }
}

public class GatewayMutation
{
    public async Task<string> ForwardMutation(
        [Service] GraphQLSchemaProxy proxy,
        string service,
        string mutation,
        string? variables = null)
    {
        return await proxy.ForwardQueryAsync(service, mutation, variables);
    }
}

public record ServiceInfo(string Name, string Url, string Description);
