using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ReviewService.Functions;

/// <summary>
/// HTTP-triggered Azure Function for on-demand feedback report generation.
/// </summary>
public class FeedbackReportFunction
{
    private readonly ILogger<FeedbackReportFunction> _logger;

    public FeedbackReportFunction(ILogger<FeedbackReportFunction> logger)
        => _logger = logger;

    [Function("GenerateFeedbackReport")]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "reports/feedback/{courseId}")] HttpRequestData req,
        long courseId)
    {
        _logger.LogInformation("Generating feedback report for Course {CourseId}", courseId);

        // TODO: Retrieve feedback data, generate report, store in blob
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new
        {
            CourseId = courseId,
            GeneratedAt = DateTime.UtcNow,
            Status = "Report generation started"
        });
        return response;
    }
}
