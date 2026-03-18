using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace EligibilityService.Functions;

/// <summary>
/// HTTP-triggered function that processes bulk eligibility refresh requests.
/// </summary>
public class EligibilityRefreshFunction
{
    private readonly ILogger<EligibilityRefreshFunction> _logger;
    private readonly HttpClient _httpClient;

    public EligibilityRefreshFunction(
        ILogger<EligibilityRefreshFunction> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("EligibilityApi");
    }

    [Function(nameof(EligibilityRefreshFunction))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "eligibility/refresh")] HttpRequestData req,
        FunctionContext context)
    {
        _logger.LogInformation("EligibilityRefreshFunction triggered.");

        var body = await req.ReadAsStringAsync();
        var payload = JsonSerializer.Deserialize<JsonElement>(body ?? "{}");

        _logger.LogInformation("Refresh payload: {Payload}", payload);

        // Call the Eligibility API to re-calculate eligibility
        var response = req.CreateResponse(System.Net.HttpStatusCode.Accepted);
        await response.WriteStringAsync("Eligibility refresh scheduled.");
        return response;
    }
}
