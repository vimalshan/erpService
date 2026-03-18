using ExitManagement.Application.DTOs;
using ExitManagement.Application.Features.EmployeeExits.Commands;
using ExitManagement.Application.Features.EmployeeExits.Queries;
using ExitManagement.Application.Features.ExitInterviews.Queries;
using ExitManagement.Application.Features.ExitQuestions.Queries;
using MediatR;

namespace ExitManagement.API.GraphQL;

public class ExitQuery
{
    public async Task<IEnumerable<EmployeeExitDto>> GetAllExitsAsync([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllExitsQuery(), ct);

    public async Task<EmployeeExitDto?> GetExitByIdAsync(decimal exitNo, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetExitByIdQuery(exitNo), ct);

    public async Task<IEnumerable<EmployeeExitDto>> GetExitsByEmployeeAsync(decimal employeeSysId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetExitsByEmployeeQuery(employeeSysId), ct);

    public async Task<IEnumerable<ExitInterviewFeedbackDto>> GetInterviewFeedbackAsync(decimal exitNo, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetInterviewFeedbackQuery(exitNo), ct);

    public async Task<IEnumerable<ExitQuestionDto>> GetExitQuestionsAsync([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllExitQuestionsQuery(), ct);

    public async Task<IEnumerable<ExitInterviewQuestionDto>> GetInterviewQuestionsAsync([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllInterviewQuestionsQuery(), ct);
}
