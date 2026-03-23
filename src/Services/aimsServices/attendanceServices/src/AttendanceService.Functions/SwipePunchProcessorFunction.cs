using AttendanceService.Application.Commands.SwipePunch;
using AttendanceService.Infrastructure.EventBus.RabbitMQ;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AttendanceService.Functions;

/// <summary>
/// Background worker: Consumes swipe punch messages from RabbitMQ queue.
/// When deployed to Azure Functions, replace with QueueTrigger-decorated function targeting Azure Storage Queue.
/// Queue: swipe-punch-queue
/// </summary>
public class SwipePunchProcessorFunction(
    EventBusRabbitMQ eventBus,
    IMediator mediator,
    ILogger<SwipePunchProcessorFunction> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("SwipePunchProcessorFunction background worker started.");

        await eventBus.SubscribeAsync(
            "attendance.swipe.queue",
            "attendance.exchange",
            "attendance.swipe.#",
            async message =>
            {
                logger.LogInformation("Received: {Message}", message);
                var payload = JsonSerializer.Deserialize<SwipePunchMessage>(message);
                if (payload is null) return;

                var cmd = new RecordSwipePunchCommand(
                    payload.EmpSysId,
                    payload.PunchTime,
                    payload.GateNo,
                    payload.PunchStatus);

                var result = await mediator.Send(cmd, stoppingToken);
                logger.LogInformation("SwipePunch recorded via worker. SwipeId={SwipeId}", result.SwipeId);
            });

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private record SwipePunchMessage(long EmpSysId, DateTime PunchTime, string GateNo, string PunchStatus);
}
