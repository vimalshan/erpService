using ExitManagement.Application.DTOs;
using ExitManagement.Domain.Interfaces;
using MediatR;

namespace ExitManagement.Application.Features.EmployeeExits.Queries;

public record GetExitByIdQuery(decimal ExitNo) : IRequest<EmployeeExitDto?>;

public class GetExitByIdQueryHandler : IRequestHandler<GetExitByIdQuery, EmployeeExitDto?>
{
    private readonly IEmployeeExitRepository _repository;

    public GetExitByIdQueryHandler(IEmployeeExitRepository repository)
        => _repository = repository;

    public async Task<EmployeeExitDto?> Handle(GetExitByIdQuery request, CancellationToken cancellationToken)
    {
        var e = await _repository.GetByIdAsync(request.ExitNo, cancellationToken);
        if (e is null) return null;

        return new EmployeeExitDto(
            e.ExitNo, e.EmployeeSysId, e.LetterGivenOn, e.ExpectedRelieveDate,
            e.ResignationType, e.ResignationId, e.Remarks, e.Status,
            e.RelieveGivenOn, e.InterviewCondductedOn, e.InterviewConductedBy,
            e.RevokeReason, e.RevokeDate, e.ApprovalStatus, e.ApprovedBy, e.ApprovedOn,
            e.PayrollSettlement, e.StopSalaryDate, e.DesignationOnJoining, e.ReasonForLeaving);
    }
}
