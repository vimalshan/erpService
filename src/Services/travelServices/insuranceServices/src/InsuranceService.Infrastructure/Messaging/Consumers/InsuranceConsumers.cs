using InsuranceService.Application.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace InsuranceService.Infrastructure.Messaging.Consumers;

public record InsuranceRegistrationMessage(
    string CompanyCode,
    long PlanNumber,
    string InsuranceType,
    string? PassportNumber,
    string? VisaPlace,
    string? Nominee1,
    string? Nominee2,
    string? Remarks);

public class InsuranceRegistrationConsumer : RabbitMqConsumerBase<InsuranceRegistrationMessage>
{
    protected override string QueueName => "insurance.registration";
    protected override string Exchange => "insurance.exchange";
    protected override string RoutingKey => "insurance.register";

    public InsuranceRegistrationConsumer(
        IConnection connection,
        IServiceScopeFactory scopeFactory,
        ILogger<InsuranceRegistrationConsumer> logger)
        : base(connection, scopeFactory, logger) { }

    protected override async Task HandleMessageAsync(
        InsuranceRegistrationMessage message,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        await mediator.Send(new RegisterInsuranceCommand(
            message.CompanyCode,
            message.PlanNumber,
            message.InsuranceType,
            message.PassportNumber,
            message.VisaPlace,
            message.Nominee1,
            message.Nominee2,
            message.Remarks), cancellationToken);
    }
}

public record InsuranceStatusUpdateMessage(
    string CompanyCode,
    long PlanNumber,
    string Status,
    string? CertificateNumber,
    long? UpdatedBy);

public class InsuranceStatusUpdateConsumer : RabbitMqConsumerBase<InsuranceStatusUpdateMessage>
{
    protected override string QueueName => "insurance.status-update";
    protected override string Exchange => "insurance.exchange";
    protected override string RoutingKey => "insurance.status.update";

    public InsuranceStatusUpdateConsumer(
        IConnection connection,
        IServiceScopeFactory scopeFactory,
        ILogger<InsuranceStatusUpdateConsumer> logger)
        : base(connection, scopeFactory, logger) { }

    protected override async Task HandleMessageAsync(
        InsuranceStatusUpdateMessage message,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        await mediator.Send(new UpdateInsuranceStatusCommand(
            message.CompanyCode,
            message.PlanNumber,
            message.Status,
            message.CertificateNumber,
            message.UpdatedBy), cancellationToken);
    }
}
