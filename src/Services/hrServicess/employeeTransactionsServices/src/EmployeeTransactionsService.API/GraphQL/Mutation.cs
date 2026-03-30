using EmployeeTransactionsService.Application.Features.AlertGroups.Commands;
using EmployeeTransactionsService.Application.Features.Employees.Commands;
using MediatR;

namespace EmployeeTransactionsService.API.GraphQL;

public sealed class Mutation
{
    public async Task<decimal> CreateEmployee([Service] IMediator mediator, CreateEmployeeCommand input, CancellationToken cancellationToken = default)
        => await mediator.Send(input, cancellationToken);

    public async Task<decimal> RegisterGradeChange([Service] IMediator mediator, RegisterGradeChangeCommand input, CancellationToken cancellationToken = default)
        => await mediator.Send(input, cancellationToken);

    public async Task<bool> ReviewProbation([Service] IMediator mediator, ReviewProbationCommand input, CancellationToken cancellationToken = default)
        => await mediator.Send(input, cancellationToken);

    public async Task<decimal> CreateAlertGroup([Service] IMediator mediator, CreateAlertGroupCommand input, CancellationToken cancellationToken = default)
        => await mediator.Send(input, cancellationToken);
}