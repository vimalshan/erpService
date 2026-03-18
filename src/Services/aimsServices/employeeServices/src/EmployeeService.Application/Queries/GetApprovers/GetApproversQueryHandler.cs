using MediatR;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.Application.Queries.GetApprovers;

public sealed class GetApproversByEmployeeQueryHandler
    : IRequestHandler<GetApproversByEmployeeQuery, IEnumerable<EmployeeApproverDto>>
{
    private readonly IEmployeeApproverRepository _repo;
    public GetApproversByEmployeeQueryHandler(IEmployeeApproverRepository repo) => _repo = repo;

    public async Task<IEnumerable<EmployeeApproverDto>> Handle(GetApproversByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetByEmployeeIdAsync(request.EmpSysId, cancellationToken);
        return items.Select(a => new EmployeeApproverDto(
            a.ApproverId, a.EmpSysId.Value, a.Level.Value,
            a.ApproverSysId, a.EffDate, a.LastModifiedBy, a.LastModifiedOn));
    }
}
