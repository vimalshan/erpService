using MediatR;
using TrainingDevelopment.Application.Common.Exceptions;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Commands.DeleteTrainingDetail;

public class DeleteTrainingDetailCommandHandler : IRequestHandler<DeleteTrainingDetailCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTrainingDetailCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteTrainingDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.TrainingDetails.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TrainingDetail), request.Id);

        _unitOfWork.TrainingDetails.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
