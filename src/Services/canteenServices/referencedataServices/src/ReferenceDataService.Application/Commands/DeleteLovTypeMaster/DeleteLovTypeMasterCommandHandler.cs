using MediatR;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Commands.DeleteLovTypeMaster;

public class DeleteLovTypeMasterCommandHandler : IRequestHandler<DeleteLovTypeMasterCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLovTypeMasterCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteLovTypeMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.LovTypeMasters.GetByCodeAsync(request.LovTypeCode, cancellationToken);
        if (entity == null) return false;

        _unitOfWork.LovTypeMasters.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
