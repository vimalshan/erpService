using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace TourServices.Infrastructure.Messaging.Consumers;

public sealed record ParticipantRegisteredMessage(long RegistrationId, long TourId, long ParticipantId, DateOnly RegistrationDate);

public sealed class ParticipantRegisteredConsumer : BaseMessageConsumer<ParticipantRegisteredMessage>
{
    protected override string QueueName => "tour.participant.registered";
    protected override string ExchangeName => "tour.events";
    protected override string RoutingKey => "tour.participant.registered";

    public ParticipantRegisteredConsumer(
        IConnection connection, ILogger<ParticipantRegisteredConsumer> logger) : base(connection, logger) { }

    protected override async Task HandleMessageAsync(
        ParticipantRegisteredMessage message, CancellationToken cancellationToken)
    {
        Logger.LogInformation(
            "Participant registered: RegistrationId={RegistrationId}, TourId={TourId}, ParticipantId={ParticipantId}",
            message.RegistrationId, message.TourId, message.ParticipantId);
        // Send confirmation email, update participant records, etc.
        await Task.CompletedTask;
    }
}
