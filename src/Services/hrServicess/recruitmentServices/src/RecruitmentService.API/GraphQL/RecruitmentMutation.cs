using HotChocolate;
using MassTransit;
using MediatR;
using RecruitmentService.Application.Commands.Applications;
using RecruitmentService.Application.Commands.Vacancies;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Infrastructure.Messaging.Consumers;

namespace RecruitmentService.API.GraphQL;

public class RecruitmentMutation
{
    /// <summary>Close a vacancy.</summary>
    public async Task<bool> CloseVacancy(
        decimal vacancyId, decimal modifiedBy,
        [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(new CloseVacancyCommand(vacancyId, modifiedBy), ct);
        return true;
    }

    /// <summary>Submit a job application.</summary>
    public async Task<decimal> SubmitApplication(
        SubmitApplicationRequest request, decimal submittedBy,
        [Service] IMediator mediator,
        [Service] IPublishEndpoint publishEndpoint,
        CancellationToken ct)
    {
        var appId = await mediator.Send(new SubmitApplicationCommand(request, submittedBy), ct);
        await publishEndpoint.Publish(new ApplicationSubmittedMessage(appId, request.VacancyId, DateTime.UtcNow), ct);
        return appId;
    }

    /// <summary>Update application status.</summary>
    public async Task<bool> UpdateApplicationStatus(
        decimal appId, string statusCode, string? remarks, decimal updatedBy,
        [Service] IMediator mediator,
        [Service] IPublishEndpoint publishEndpoint,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateApplicationStatusCommand(appId, statusCode, remarks, updatedBy), ct);
        await publishEndpoint.Publish(new ApplicationStatusChangedMessage(appId, "UNKNOWN", statusCode, DateTime.UtcNow), ct);
        return true;
    }
}
