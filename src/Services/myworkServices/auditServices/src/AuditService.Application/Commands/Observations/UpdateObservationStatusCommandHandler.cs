using AuditService.Domain.Interfaces;
using MediatR;

namespace AuditService.Application.Commands.Observations;

public sealed class UpdateObservationStatusCommandHandler : IRequestHandler<UpdateObservationStatusCommand, bool>
{
    private readonly IObservationRepository _observationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateObservationStatusCommandHandler(IObservationRepository observationRepository, IUnitOfWork unitOfWork)
    {
        _observationRepository = observationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateObservationStatusCommand request, CancellationToken cancellationToken)
    {
        var observation = await _observationRepository.GetByIdAsync(request.ObvId, cancellationToken);
        if (observation is null) return false;

        observation.UpdateStatus(request.NewStatus, request.ModifiedBy);
        await _observationRepository.UpdateAsync(observation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
