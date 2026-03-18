using ExitManagement.Application.Common.Interfaces;
using ExitManagement.Application.DTOs;
using ExitManagement.Domain.Interfaces;
using MediatR;

namespace ExitManagement.Application.Features.EmployeeExits.Queries;

public record GetAllExitsQuery : IRequest<IEnumerable<EmployeeExitDto>>;

public class GetAllExitsQueryHandler : IRequestHandler<GetAllExitsQuery, IEnumerable<EmployeeExitDto>>
{
    private readonly IEmployeeExitRepository _repository;

    public GetAllExitsQueryHandler(IEmployeeExitRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<EmployeeExitDto>> Handle(GetAllExitsQuery request, CancellationToken cancellationToken)
    {
        var exits = await _repository.GetAllAsync(cancellationToken);
        return exits.Select(e => new EmployeeExitDto(
            e.ExitNo, e.EmployeeSysId, e.LetterGivenOn, e.ExpectedRelieveDate,
            e.ResignationType, e.ResignationId, e.Remarks, e.Status,
            e.RelieveGivenOn, e.InterviewCondductedOn, e.InterviewConductedBy,
            e.RevokeReason, e.RevokeDate, e.ApprovalStatus, e.ApprovedBy, e.ApprovedOn,
            e.PayrollSettlement, e.StopSalaryDate, e.DesignationOnJoining, e.ReasonForLeaving));
    }
}
