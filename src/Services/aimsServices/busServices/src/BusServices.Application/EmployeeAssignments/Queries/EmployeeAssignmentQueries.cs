using MediatR;
using BusServices.Application.DTOs;
using BusServices.Domain.Interfaces;

namespace BusServices.Application.EmployeeAssignments.Queries;

public record GetAssignmentsByEmployeeQuery(long EmpSysId) : IRequest<IEnumerable<EmployeeBusDto>>;

public sealed class GetAssignmentsByEmployeeQueryHandler : IRequestHandler<GetAssignmentsByEmployeeQuery, IEnumerable<EmployeeBusDto>>
{
    private readonly IEmployeeBusRepository _repo;

    public GetAssignmentsByEmployeeQueryHandler(IEmployeeBusRepository repo) => _repo = repo;

    public async Task<IEnumerable<EmployeeBusDto>> Handle(GetAssignmentsByEmployeeQuery request, CancellationToken ct)
    {
        var items = await _repo.GetByEmployeeIdAsync(request.EmpSysId, ct);
        return items.Select(a => new EmployeeBusDto(
            a.EmpBusId, a.EmpSysId, a.BusId, a.RouteId,
            a.EffectiveDate, a.ClosingDate, a.LastModifiedBy, a.LastModifiedOn));
    }
}
