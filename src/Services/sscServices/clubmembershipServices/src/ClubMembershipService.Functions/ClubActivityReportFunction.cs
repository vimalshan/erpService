using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ClubMembershipService.Functions;

public class ClubActivityReportFunction
{
    private readonly ILogger<ClubActivityReportFunction> _logger;

    public ClubActivityReportFunction(ILogger<ClubActivityReportFunction> logger)
        => _logger = logger;

    [Function("GenerateMonthlyActivityReport")]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "reports/monthly")] HttpRequestData req)
    {
        _logger.LogInformation("Generating monthly activity report at: {Time}", DateTime.UtcNow);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new
        {
            reportDate = DateTime.UtcNow,
            message = "Monthly activity report generated successfully",
            period = $"{DateTime.UtcNow.Year}-{DateTime.UtcNow.Month:D2}"
        });

        return response;
    }
}
