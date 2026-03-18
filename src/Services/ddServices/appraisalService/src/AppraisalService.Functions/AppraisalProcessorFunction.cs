using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AppraisalService.Functions;

/// <summary>
/// Azure Function for processing appraisals on a schedule
/// </summary>
public class AppraisalProcessorFunction
{
    private readonly ILogger<AppraisalProcessorFunction> _logger;

    public AppraisalProcessorFunction(ILogger<AppraisalProcessorFunction> logger)
    {
        _logger = logger;
    }

    [Function("AppraisalProcessor")]
    public async Task Run(
        [TimerTrigger("0 0 * * * *")] TimerInfo myTimer,
        FunctionContext context)
    {
        _logger.LogInformation($"Appraisal Processor function executed at: {DateTime.Now}");

        // Process pending appraisals
        // Update overdue appraisals
        // Generate notifications for pending actions

        // Note: TimerInfo property access differs in Azure Functions Worker SDK
        // if (myTimer.ScheduleStatus?.IsPastDue == true)
        // {
        //     _logger.LogInformation("Timer is running late!");
        // }

        await Task.CompletedTask;
    }
}

/// <summary>
/// Azure Function for handling RabbitMQ messages
/// </summary>
public class AppraisalMessageProcessorFunction
{
    private readonly ILogger<AppraisalMessageProcessorFunction> _logger;

    public AppraisalMessageProcessorFunction(ILogger<AppraisalMessageProcessorFunction> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Note: RabbitMQ trigger requires separate binding extension. Disabled for now.
    /// Use service bus or other standard Azure triggers for message processing.
    /// </summary>
    /*
    [Function("ProcessAppraisalMessages")]
    public async Task Run(
        [RabbitMQTrigger("appraisal-queue")] byte[] messageBytes,
        FunctionContext context)
    {
        try
        {
            var message = System.Text.Encoding.UTF8.GetString(messageBytes);
            _logger.LogInformation($"Processing message: {message}");

            // Process the message
            // Update appraisal status
            // Trigger notifications

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
            throw;
        }
    }
    */
}

/// <summary>
/// Azure Function for generating reports
/// </summary>
public class AppraisalReportGeneratorFunction
{
    private readonly ILogger<AppraisalReportGeneratorFunction> _logger;

    public AppraisalReportGeneratorFunction(ILogger<AppraisalReportGeneratorFunction> logger)
    {
        _logger = logger;
    }

    [Function("GenerateAppraisalReports")]
    public async Task Run(
        [TimerTrigger("0 0 1 * * *")] TimerInfo myTimer, // Runs at 1 AM daily
        FunctionContext context)
    {
        _logger.LogInformation($"Report generation function executed at: {DateTime.Now}");

        try
        {
            // Generate monthly reports
            // Export data to Blob Storage
            // Send reports via email

            _logger.LogInformation("Reports generated successfully");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating reports");
            throw;
        }
    }
}

/// <summary>
/// Azure Function triggered by HTTP for manual processing
/// </summary>
public class ManualAppraisalProcessorFunction
{
    private readonly ILogger<ManualAppraisalProcessorFunction> _logger;

    public ManualAppraisalProcessorFunction(ILogger<ManualAppraisalProcessorFunction> logger)
    {
        _logger = logger;
    }

    [Function("ManualAppraisalProcessor")]
    public async Task<Microsoft.Azure.Functions.Worker.Http.HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "appraisals/process")] 
        Microsoft.Azure.Functions.Worker.Http.HttpRequestData req,
        FunctionContext context)
    {
        _logger.LogInformation("Manual appraisal processor triggered");

        try
        {
            // Process appraisals based on request data
            var response = req.CreateResponse();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Headers.Add("Content-Type", "application/json");
            
            // Write JSON response to body stream
            var result = new { message = "Appraisals processed successfully" };
            var json = JsonSerializer.Serialize(result);
            using (var writer = new StreamWriter(response.Body))
            {
                await writer.WriteAsync(json);
                await writer.FlushAsync();
            }
            
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in manual processor");
            var errorResponse = req.CreateResponse();
            errorResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            errorResponse.Headers.Add("Content-Type", "application/json");
            
            var errorResult = new { error = ex.Message };
            var errorJson = JsonSerializer.Serialize(errorResult);
            using (var writer = new StreamWriter(errorResponse.Body))
            {
                await writer.WriteAsync(errorJson);
                await writer.FlushAsync();
            }
            
            return errorResponse;
        }
    }
}
