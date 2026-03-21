using MediatR;
using UnitService.Domain.Entities;
using UnitService.Domain.Interfaces;

namespace UnitService.Application.Commands.RegisterEquipment;

public class RegisterEquipmentCommandHandler : IRequestHandler<RegisterEquipmentCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterEquipmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(RegisterEquipmentCommand request, CancellationToken cancellationToken)
    {
        var existing = await _unitOfWork.Equipment.GetByIdAsync(request.EquipmentId, cancellationToken);

        if (existing is not null)
        {
            existing.Update(request.EquipmentName, request.Category, request.ModifiedBy);
            _unitOfWork.Equipment.Update(existing);
        }
        else
        {
            var equipment = EquipmentMaster.Create(
                request.EquipmentId,
                request.EquipmentName,
                request.UnitCode,
                request.Category,
                request.ModifiedBy);

            await _unitOfWork.Equipment.AddAsync(equipment, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return request.EquipmentId;
    }
}
