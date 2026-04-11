using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TransactionService.Infrastructure.Messaging.Consumers;

public sealed record JVPostedMessage(long JvBatchId, string JvType, string OracleRefNo, long PostedBy, DateTime PostedOn);

public sealed class JVPostedConsumer : BaseMessageConsumer<JVPostedMessage>
{
    protected override string QueueName => "transaction.jv.posted";
    protected override string ExchangeName => "transaction.events";
    protected override string RoutingKey => "transaction.jv.posted";

    public JVPostedConsumer(
        IConfiguration configuration, ILogger<JVPostedConsumer> logger) : base(configuration, logger) { }

    protected override async Task HandleMessageAsync(
        JVPostedMessage message, CancellationToken cancellationToken)
    {
        Logger.LogInformation(
            "JV Posted: JvBatchId={JvBatchId}, Type={JvType}, OracleRef={OracleRefNo}",
            message.JvBatchId, message.JvType, message.OracleRefNo);
        await Task.CompletedTask;
    }
}
