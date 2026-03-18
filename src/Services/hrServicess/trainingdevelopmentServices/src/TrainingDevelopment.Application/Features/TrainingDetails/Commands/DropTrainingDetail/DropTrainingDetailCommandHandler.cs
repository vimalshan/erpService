using MediatR;
using TrainingDevelopment.Application.Common.Exceptions;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Commands.DropTrainingDetail;

public class DropTrainingDetailCommandHandler : IRequestHandler<DropTrainingDetailCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DropTrainingDetailCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DropTrainingDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.TrainingDetails.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TrainingDetail), request.Id);

        entity.Drop(request.Remarks);
        _unitOfWork.TrainingDetails.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
