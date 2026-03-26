using MediatR;
using ReferenceDataService.Domain.Events;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Commands.DeleteLovMaster;

public class DeleteLovMasterCommandHandler : IRequestHandler<DeleteLovMasterCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLovMasterCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteLovMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.LovMasters.GetByIdAsync(request.LovId, cancellationToken);
        if (entity == null) return false;

        entity.AddDomainEvent(new LovMasterDeletedEvent(request.LovId));
        _unitOfWork.LovMasters.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
