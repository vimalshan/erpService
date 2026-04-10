using MediatR;
using Microsoft.Extensions.Logging;
using SparshTransactional.Domain.Events;
using SparshTransactional.Domain.Interfaces;

namespace SparshTransactional.Infrastructure.EventHandlers;

public class ScholarshipCreatedEventHandler(
    IMessagePublisher publisher,
    ILogger<ScholarshipCreatedEventHandler> logger) : INotificationHandler<ScholarshipCreatedEvent>
{
    public async Task Handle(ScholarshipCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Scholarship {Id} '{Name}' created",
            notification.Scholarship.ScholarshipId, notification.Scholarship.ScholarshipName);

        await publisher.PublishAsync("scholarship.exchange", "scholarship.created", new
        {
            notification.Scholarship.ScholarshipId,
            notification.Scholarship.ScholarshipName,
            notification.Scholarship.ScholarshipType
        }, ct);
    }
}

public class ApplicationSubmittedEventHandler(
    IMessagePublisher publisher,
    ILogger<ApplicationSubmittedEventHandler> logger) : INotificationHandler<ApplicationSubmittedEvent>
{
    public async Task Handle(ApplicationSubmittedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Application {Id} submitted by Student {StudentId}",
            notification.Application.ApplicationId, notification.Application.StudentId);

        await publisher.PublishAsync("scholarship.exchange", "application.submitted", new
        {
            notification.Application.ApplicationId,
            notification.Application.StudentId,
            notification.Application.ScholarshipId
        }, ct);
    }
}

public class ApplicationApprovedEventHandler(
    IMessagePublisher publisher,
    ILogger<ApplicationApprovedEventHandler> logger) : INotificationHandler<ApplicationApprovedEvent>
{
    public async Task Handle(ApplicationApprovedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Application {Id} approved by {ApprovedBy}, Amount: {Amount}",
            notification.Application.ApplicationId, notification.ApprovedBy, notification.Application.ApprovedAmount);

        await publisher.PublishAsync("scholarship.exchange", "application.approved", new
        {
            notification.Application.ApplicationId,
            notification.Application.StudentId,
            notification.Application.ScholarshipId,
            notification.Application.ApprovedAmount,
            notification.ApprovedBy
        }, ct);
    }
}

public class ApplicationRejectedEventHandler(
    IMessagePublisher publisher,
    ILogger<ApplicationRejectedEventHandler> logger) : INotificationHandler<ApplicationRejectedEvent>
{
    public async Task Handle(ApplicationRejectedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Application {Id} rejected by {RejectedBy}",
            notification.Application.ApplicationId, notification.RejectedBy);

        await publisher.PublishAsync("scholarship.exchange", "application.rejected", new
        {
            notification.Application.ApplicationId,
            notification.Application.StudentId,
            notification.RejectedBy,
            notification.Reason
        }, ct);
    }
}

public class DisbursementCreatedEventHandler(
    IMessagePublisher publisher,
    ILogger<DisbursementCreatedEventHandler> logger) : INotificationHandler<DisbursementCreatedEvent>
{
    public async Task Handle(DisbursementCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Disbursement {Id} created for Application {AppId}, Amount: {Amount}",
            notification.Disbursement.DisbursementId, notification.Disbursement.ApplicationId,
            notification.Disbursement.DisbursementAmount);

        await publisher.PublishAsync("scholarship.exchange", "disbursement.created", new
        {
            notification.Disbursement.DisbursementId,
            notification.Disbursement.ApplicationId,
            notification.Disbursement.StudentId,
            notification.Disbursement.DisbursementAmount
        }, ct);
    }
}

public class DisbursementCompletedEventHandler(
    IMessagePublisher publisher,
    ILogger<DisbursementCompletedEventHandler> logger) : INotificationHandler<DisbursementCompletedEvent>
{
    public async Task Handle(DisbursementCompletedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Disbursement {Id} completed, Ref: {Ref}",
            notification.Disbursement.DisbursementId, notification.Disbursement.PaymentReference);

        await publisher.PublishAsync("scholarship.exchange", "disbursement.completed", new
        {
            notification.Disbursement.DisbursementId,
            notification.Disbursement.StudentId,
            notification.Disbursement.DisbursementAmount,
            notification.Disbursement.PaymentReference
        }, ct);
    }
}
