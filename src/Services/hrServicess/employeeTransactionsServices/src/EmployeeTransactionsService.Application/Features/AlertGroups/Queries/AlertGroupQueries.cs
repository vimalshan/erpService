using EmployeeTransactionsService.Application.DTOs;
using EmployeeTransactionsService.Domain.Interfaces;
using MediatR;

namespace EmployeeTransactionsService.Application.Features.AlertGroups.Queries;

public sealed record GetAlertGroupByIdQuery(decimal AlertGroupId) : IRequest<AlertGroupDto?>;

public sealed class GetAlertGroupByIdQueryHandler(IAlertGroupRepository alertGroupRepository)
    : IRequestHandler<GetAlertGroupByIdQuery, AlertGroupDto?>
{
    public async Task<AlertGroupDto?> Handle(GetAlertGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var group = await alertGroupRepository.GetByIdAsync(request.AlertGroupId, cancellationToken);
        return group?.ToDto();
    }
}

public sealed record ListAlertGroupsQuery() : IRequest<IReadOnlyList<AlertGroupDto>>;

public sealed class ListAlertGroupsQueryHandler(IAlertGroupRepository alertGroupRepository)
    : IRequestHandler<ListAlertGroupsQuery, IReadOnlyList<AlertGroupDto>>
{
    public async Task<IReadOnlyList<AlertGroupDto>> Handle(ListAlertGroupsQuery request, CancellationToken cancellationToken)
        => (await alertGroupRepository.ListAsync(cancellationToken)).Select(group => group.ToDto()).ToList();
}