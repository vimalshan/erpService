using GroupIncentiveService.Domain.Entities;
using GroupIncentiveService.Domain.Exceptions;
using GroupIncentiveService.Domain.Interfaces;
using MediatR;

namespace GroupIncentiveService.Application.Commands.AddEmployeeToGroup;

public class AddEmployeeToGroupHandler : IRequestHandler<AddEmployeeToGroupCommand, long>
{
    private readonly IGroupMasterRepository _groupRepo;
    private readonly IGroupEmployeeMapRepository _mapRepo;
    private readonly IUnitOfWork _unitOfWork;

    public AddEmployeeToGroupHandler(IGroupMasterRepository groupRepo,
        IGroupEmployeeMapRepository mapRepo, IUnitOfWork unitOfWork)
    {
        _groupRepo = groupRepo;
        _mapRepo = mapRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(AddEmployeeToGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepo.GetByIdAsync(request.GroupId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.GroupMaster), request.GroupId);

        if (!group.IsActive)
            throw new BusinessRuleViolationException("Cannot add employee to an inactive group.");

        var existing = await _mapRepo.GetByGroupIdAsync(request.GroupId, cancellationToken);
        if (existing.Any(m => m.GrpEmpMapEmpSysId == request.EmployeeId && m.IsActive))
            throw new BusinessRuleViolationException("Employee is already an active member of this group.");

        var nextId = await _mapRepo.GetNextIdAsync(cancellationToken);
        var mapping = GroupEmployeeMap.Create(nextId, request.GroupId, request.EmployeeId,
            request.EffDate, request.Role, request.CreatedBy);

        await _mapRepo.AddAsync(mapping, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return mapping.GrpEmpMapId;
    }
}
