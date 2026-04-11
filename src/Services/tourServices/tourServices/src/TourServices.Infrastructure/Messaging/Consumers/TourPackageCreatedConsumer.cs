using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TourServices.Application.TourPackages.Commands.ChangeTourStatus;

namespace TourServices.Infrastructure.Messaging.Consumers;

public sealed record TourPackageCreatedMessage(long TourId, string TourName, string Destination, long PlannedBy);

public sealed class TourPackageCreatedConsumer : BaseMessageConsumer<TourPackageCreatedMessage>
{
    private readonly IMediator _mediator;

    protected override string QueueName => "tour.package.created";
    protected override string ExchangeName => "tour.events";
    protected override string RoutingKey => "tour.package.created";

    public TourPackageCreatedConsumer(IConfiguration configuration, IMediator mediator,
        ILogger<TourPackageCreatedConsumer> logger) : base(configuration, logger)
    {
        _mediator = mediator;
    }

    protected override async Task HandleMessageAsync(
        TourPackageCreatedMessage message, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Tour package created event received: TourId={TourId}, Name={TourName}",
            message.TourId, message.TourName);
        // Additional processing: notify planners, send welcome email, etc.
        await Task.CompletedTask;
    }
}
