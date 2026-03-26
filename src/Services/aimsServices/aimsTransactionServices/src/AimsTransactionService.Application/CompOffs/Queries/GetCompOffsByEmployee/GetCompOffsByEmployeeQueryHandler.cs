using MediatR;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Application.DTOs;
using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Application.CompOffs.Queries.GetCompOffsByEmployee;

public sealed class GetCompOffsByEmployeeQueryHandler(ICompOffRepository compOffRepository)
    : IRequestHandler<GetCompOffsByEmployeeQuery, IEnumerable<CompOffDto>>
{
    public async Task<IEnumerable<CompOffDto>> Handle(
        GetCompOffsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var compOffs = await compOffRepository.GetByEmployeeAsync(request.EmployeeSysId, cancellationToken);
        return compOffs.Select(MapToDto);
    }

    private static CompOffDto MapToDto(CompOffAggregate c) => new(
        c.Id,
        c.EmployeeSysId,
        c.RequestedOn,
        c.HoursRequested,
        c.Status.ToString(),
        c.RequestedBy,
        c.RequestedOn);
}
