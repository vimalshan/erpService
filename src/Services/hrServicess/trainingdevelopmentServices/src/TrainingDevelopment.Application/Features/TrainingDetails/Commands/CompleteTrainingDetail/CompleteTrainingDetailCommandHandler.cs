using MediatR;
using TrainingDevelopment.Application.Common.Exceptions;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Commands.CompleteTrainingDetail;

public class CompleteTrainingDetailCommandHandler : IRequestHandler<CompleteTrainingDetailCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CompleteTrainingDetailCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(CompleteTrainingDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.TrainingDetails.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TrainingDetail), request.Id);

        entity.MarkCompleted(
            request.ActualFrom, request.ActualTo,
            request.InstituteId, request.InstituteDescription,
            request.TrainerId, request.TrainerDescription,
            request.PlaceId, request.Place, request.Cost);

        _unitOfWork.TrainingDetails.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
