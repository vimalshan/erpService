using MassTransit;
using Microsoft.Extensions.Logging;

namespace Document.Infrastructure.Consumers;

// Message contracts
public record LetterGeneratedMessage(decimal? EmployeePin, string? LetterType, DateTime? GeneratedAt);
public record LetterOpenedMessage(decimal EmployeeSysId, string LetterType, string IpAddress);

public class LetterGeneratedConsumer : IConsumer<LetterGeneratedMessage>
{
    private readonly ILogger<LetterGeneratedConsumer> _logger;

    public LetterGeneratedConsumer(ILogger<LetterGeneratedConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<LetterGeneratedMessage> context)
    {
        _logger.LogInformation(
            "Consumed LetterGenerated event: EmployeePin={Pin}, LetterType={Type}, GeneratedAt={At}",
            context.Message.EmployeePin, context.Message.LetterType, context.Message.GeneratedAt);
        // TODO: trigger downstream workflows (e.g., send email notification, generate PDF)
        return Task.CompletedTask;
    }
}

public class LetterOpenedConsumer : IConsumer<LetterOpenedMessage>
{
    private readonly ILogger<LetterOpenedConsumer> _logger;

    public LetterOpenedConsumer(ILogger<LetterOpenedConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<LetterOpenedMessage> context)
    {
        _logger.LogInformation(
            "Consumed LetterOpened event: EmployeeSysId={Id}, LetterType={Type}, IP={IP}",
            context.Message.EmployeeSysId, context.Message.LetterType, context.Message.IpAddress);
        return Task.CompletedTask;
    }
}
