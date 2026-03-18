using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MediatR;
using LoanApplication.Application.Commands;
using System.Text.Json;

namespace LoanApplication.Functions.Functions;

/// <summary>
/// RabbitMQ-triggered function that consumes "loan.application.submitted" events
/// and auto-approves/escalates applications based on business rules.
/// Queue: loan-approval-queue
/// </summary>
public class LoanApplicationProcessorFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<LoanApplicationProcessorFunction> _logger;

    public LoanApplicationProcessorFunction(IMediator mediator, ILogger<LoanApplicationProcessorFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Processes incoming loan application approval requests from the queue.
    /// Message payload is a JSON-serialized <see cref="LoanApprovalRequest"/>.
    /// </summary>
    [Function(nameof(LoanApplicationProcessorFunction))]
    public async Task Run(
        [RabbitMQTrigger("loan-approval-queue", ConnectionStringSetting = "RabbitMQ__ConnectionString")] string message,
        FunctionContext context)
    {
        _logger.LogInformation("LoanApplicationProcessorFunction received message: {Message}", message);

        LoanApprovalRequest? request = null;

        try
        {
            request = JsonSerializer.Deserialize<LoanApprovalRequest>(message, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (request == null)
            {
                _logger.LogWarning("Received null or unparseable message, skipping.");
                return;
            }

            _logger.LogInformation(
                "Processing approval for LoanApplicationId={Id}, AutoApprove={Auto}",
                request.LoanApplicationId, request.AutoApprove);

            if (request.AutoApprove)
            {
                // Auto-approve low-value or fast-track applications
                var command = new ApproveLoanApplicationCommand
                {
                    LoanApplicationId = request.LoanApplicationId,
                    ApprovedBy = request.ReviewerId,
                    Remarks = "Auto-approved by background processing rules"
                };
                await _mediator.Send(command);
                _logger.LogInformation("LoanApplication {Id} auto-approved.", request.LoanApplicationId);
            }
            else
            {
                _logger.LogInformation(
                    "LoanApplication {Id} requires manual review — escalated to reviewer {ReviewerId}.",
                    request.LoanApplicationId, request.ReviewerId);
                // In production: send notification to reviewer via email/Teams
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing loan approval message for application {Id}",
                request?.LoanApplicationId);
            throw; // Re-throw to trigger dead-letter queue handling
        }
    }
}

/// <summary>
/// Message payload for the loan approval queue
/// </summary>
public class LoanApprovalRequest
{
    public long LoanApplicationId { get; set; }
    public long ReviewerId { get; set; }
    public bool AutoApprove { get; set; }
    public string? Notes { get; set; }
}
