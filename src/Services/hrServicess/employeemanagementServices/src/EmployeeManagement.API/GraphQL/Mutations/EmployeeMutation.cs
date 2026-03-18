using EmployeeManagement.Application.Employees.Commands.CreateEmployee;
using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagement.Application.Promotions.Commands.CreatePromotion;
using EmployeeManagement.Application.Promotions.DTOs;
using HotChocolate.Authorization;
using MediatR;

namespace EmployeeManagement.API.GraphQL.Mutations;

[MutationType]
public sealed class EmployeeMutation
{
    /// <summary>Create a new employee via GraphQL.</summary>
    [Authorize]
    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeCommand input,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    /// <summary>Create a promotion via GraphQL.</summary>
    [Authorize]
    public async Task<PromotionDto> CreatePromotionAsync(CreatePromotionCommand input,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);
}
