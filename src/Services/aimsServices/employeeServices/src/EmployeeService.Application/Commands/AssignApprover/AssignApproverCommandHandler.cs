using MediatR;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.Application.Commands.AssignApprover;

public sealed class AssignApproverCommandHandler : IRequestHandler<AssignApproverCommand, EmployeeApproverDto>
{
    private readonly IEmployeeApproverRepository _repo;

    public AssignApproverCommandHandler(IEmployeeApproverRepository repo) => _repo = repo;

    public async Task<EmployeeApproverDto> Handle(AssignApproverCommand request, CancellationToken cancellationToken)
    {
        var nextId = await _repo.GetNextIdAsync(cancellationToken);
        var approver = EmployeeApprover.Create(nextId, request.EmpSysId, request.Level, request.ApproverSysId, request.AssignedBy);
        await _repo.AddAsync(approver, cancellationToken);

        return new EmployeeApproverDto(
            approver.ApproverId,
            approver.EmpSysId.Value,
            approver.Level.Value,
            approver.ApproverSysId,
            approver.EffDate,
            approver.LastModifiedBy,
            approver.LastModifiedOn
        );
    }
}
