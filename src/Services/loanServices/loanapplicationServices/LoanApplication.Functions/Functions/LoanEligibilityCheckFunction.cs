using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MediatR;
using LoanApplication.Application.Queries;
using System.Net;
using System.Text.Json;

namespace LoanApplication.Functions.Functions;

/// <summary>
/// HTTP-triggered function for on-demand loan eligibility checks.
/// Endpoint:  GET /api/eligibility/{employeeId}?loanTypeId={loanTypeId}
/// </summary>
public class LoanEligibilityCheckFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<LoanEligibilityCheckFunction> _logger;

    public LoanEligibilityCheckFunction(IMediator mediator, ILogger<LoanEligibilityCheckFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Function(nameof(LoanEligibilityCheckFunction))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "eligibility/{employeeId}")] HttpRequestData req,
        long employeeId,
        FunctionContext context)
    {
        _logger.LogInformation("LoanEligibilityCheckFunction triggered for employee {EmployeeId}", employeeId);

        var query = req.Url.Query;
        long loanTypeId = 0;
        var queryParams = System.Web.HttpUtility.ParseQueryString(query);
        long.TryParse(queryParams["loanTypeId"], out loanTypeId);

        try
        {
            var result = await _mediator.Send(new CheckLoanEligibilityQuery
            {
                EmployeeId = employeeId,
                LoanTypeId = loanTypeId
            });

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking loan eligibility for employee {EmployeeId}", employeeId);
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("{\"error\":\"An error occurred while checking eligibility.\"}");
            return errorResponse;
        }
    }
}
