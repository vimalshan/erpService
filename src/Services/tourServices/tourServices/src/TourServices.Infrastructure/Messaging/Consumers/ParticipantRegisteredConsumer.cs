using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TourServices.Infrastructure.Messaging.Consumers;

public sealed record ParticipantRegisteredMessage(long RegistrationId, long TourId, long ParticipantId, DateOnly RegistrationDate);

public sealed class ParticipantRegisteredConsumer : BaseMessageConsumer<ParticipantRegisteredMessage>
{
    protected override string QueueName => "tour.participant.registered";
    protected override string ExchangeName => "tour.events";
    protected override string RoutingKey => "tour.participant.registered";

    public ParticipantRegisteredConsumer(
        IConfiguration configuration, ILogger<ParticipantRegisteredConsumer> logger) : base(configuration, logger) { }

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
