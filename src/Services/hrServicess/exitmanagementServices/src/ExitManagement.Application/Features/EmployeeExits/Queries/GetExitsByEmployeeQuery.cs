using ExitManagement.Application.DTOs;
using ExitManagement.Domain.Interfaces;
using MediatR;

namespace ExitManagement.Application.Features.EmployeeExits.Queries;

public record GetExitsByEmployeeQuery(decimal EmployeeSysId) : IRequest<IEnumerable<EmployeeExitDto>>;

public class GetExitsByEmployeeQueryHandler : IRequestHandler<GetExitsByEmployeeQuery, IEnumerable<EmployeeExitDto>>
{
    private readonly IEmployeeExitRepository _repository;

    public GetExitsByEmployeeQueryHandler(IEmployeeExitRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<EmployeeExitDto>> Handle(GetExitsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var exits = await _repository.GetByEmployeeAsync(request.EmployeeSysId, cancellationToken);
        return exits.Select(e => new EmployeeExitDto(
            e.ExitNo, e.EmployeeSysId, e.LetterGivenOn, e.ExpectedRelieveDate,
            e.ResignationType, e.ResignationId, e.Remarks, e.Status,
            e.RelieveGivenOn, e.InterviewCondductedOn, e.InterviewConductedBy,
            e.RevokeReason, e.RevokeDate, e.ApprovalStatus, e.ApprovedBy, e.ApprovedOn,
            e.PayrollSettlement, e.StopSalaryDate, e.DesignationOnJoining, e.ReasonForLeaving));
    }
}
