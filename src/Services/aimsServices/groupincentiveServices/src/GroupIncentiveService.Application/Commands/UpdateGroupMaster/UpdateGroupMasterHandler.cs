using GroupIncentiveService.Domain.Exceptions;
using GroupIncentiveService.Domain.Interfaces;
using MediatR;

namespace GroupIncentiveService.Application.Commands.UpdateGroupMaster;

public class UpdateGroupMasterHandler : IRequestHandler<UpdateGroupMasterCommand, Unit>
{
    private readonly IGroupMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGroupMasterHandler(IGroupMasterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateGroupMasterCommand request, CancellationToken cancellationToken)
    {
        var group = await _repository.GetByIdAsync(request.GroupId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.GroupMaster), request.GroupId);

        group.Update(request.GroupName, request.GroupDescription, request.ModifiedBy);
        await _repository.UpdateAsync(group, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
