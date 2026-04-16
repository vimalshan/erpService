using CertificateService.Domain.Events;
using MassTransit;

namespace CertificateService.Infrastructure.Messaging;

public class CertificateIssuedConsumer : IConsumer<CertificateIssuedEvent>
{
    private readonly ILogger<CertificateIssuedConsumer> _logger;
    public CertificateIssuedConsumer(ILogger<CertificateIssuedConsumer> logger) => _logger = logger;
    public Task Consume(ConsumeContext<CertificateIssuedEvent> context)
    { _logger.LogInformation("Certificate issued: {Id}", context.Message.CertificateId); return Task.CompletedTask; }
}
