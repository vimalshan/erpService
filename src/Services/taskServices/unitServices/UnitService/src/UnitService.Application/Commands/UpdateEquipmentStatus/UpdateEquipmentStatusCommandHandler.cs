using MediatR;
using UnitService.Domain.Entities;
using UnitService.Domain.Interfaces;

namespace UnitService.Application.Commands.UpdateEquipmentStatus;

public class UpdateEquipmentStatusCommandHandler : IRequestHandler<UpdateEquipmentStatusCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEquipmentStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(UpdateEquipmentStatusCommand request, CancellationToken cancellationToken)
    {
        var status = EquipmentStatus.Create(
            request.StatusId,
            request.EquipmentId,
            request.StatusDescription,
            request.StatusCode,
            request.Remarks,
            request.Hours,
            request.CreatedBy);

        await _unitOfWork.EquipmentStatuses.AddAsync(status, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return request.StatusId;
    }
}
