using GroupIncentiveService.Domain.Entities;
using GroupIncentiveService.Domain.Exceptions;
using GroupIncentiveService.Domain.Interfaces;
using MediatR;

namespace GroupIncentiveService.Application.Commands.CreateGroupIncentive;

public class CreateGroupIncentiveHandler : IRequestHandler<CreateGroupIncentiveCommand, long>
{
    private readonly IGroupMasterRepository _groupRepo;
    private readonly IGroupIncentiveMainRepository _mainRepo;
    private readonly IGroupIncentiveDetRepository _detRepo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGroupIncentiveHandler(IGroupMasterRepository groupRepo,
        IGroupIncentiveMainRepository mainRepo,
        IGroupIncentiveDetRepository detRepo,
        IUnitOfWork unitOfWork)
    {
        _groupRepo = groupRepo;
        _mainRepo = mainRepo;
        _detRepo = detRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(CreateGroupIncentiveCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepo.GetByIdAsync(request.GroupId, cancellationToken)
            ?? throw new NotFoundException(nameof(GroupMaster), request.GroupId);

        if (!group.IsActive)
            throw new BusinessRuleViolationException("Cannot create incentive for an inactive group.");

        var totalAllocPct = request.Details.Sum(d => d.AllocPercentage);
        if (totalAllocPct > 100)
            throw new DomainException($"Total allocation percentage {totalAllocPct} exceeds 100%.");

        var mainId = await _mainRepo.GetNextIdAsync(cancellationToken);
        var incentive = GroupIncentiveMain.Create(mainId, request.GroupId, request.Month, request.Year,
            request.TotalAmount, request.CreatedBy);

        await _mainRepo.AddAsync(incentive, cancellationToken);

        long nextDetId = await _detRepo.GetNextIdAsync(cancellationToken);
        var details = request.Details.Select((d, i) =>
            GroupIncentiveDet.Create(nextDetId + i, mainId, d.EmployeeId,
                d.AllocPercentage, d.AllocAmount, request.CreatedBy)).ToList();

        await _detRepo.AddRangeAsync(details, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return incentive.GrpIncId;
    }
}
