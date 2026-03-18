using GroupIncentiveService.Domain.Entities;
using GroupIncentiveService.Domain.Exceptions;
using GroupIncentiveService.Domain.Interfaces;
using MediatR;

namespace GroupIncentiveService.Application.Commands.CreateGroupMaster;

public class CreateGroupMasterHandler : IRequestHandler<CreateGroupMasterCommand, int>
{
    private readonly IGroupMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGroupMasterHandler(IGroupMasterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateGroupMasterCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByNameAsync(request.GroupName, cancellationToken);
        if (existing is not null)
            throw new BusinessRuleViolationException($"A group named '{request.GroupName}' already exists.");

        var nextId = await _repository.GetNextIdAsync(cancellationToken);
        var group = GroupMaster.Create(nextId, request.GroupName, request.GroupDescription,
            request.GroupEffDate, request.CreatedBy);

        await _repository.AddAsync(group, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return group.GroupId;
    }
}
