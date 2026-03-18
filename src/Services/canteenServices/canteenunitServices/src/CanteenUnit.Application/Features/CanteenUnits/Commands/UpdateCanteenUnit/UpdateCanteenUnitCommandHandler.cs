using CanteenUnit.Domain.Interfaces;
using MediatR;

namespace CanteenUnit.Application.Features.CanteenUnits.Commands.UpdateCanteenUnit;

public class UpdateCanteenUnitCommandHandler : IRequestHandler<UpdateCanteenUnitCommand>
{
    private readonly ICanteenUnitRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCanteenUnitCommandHandler(ICanteenUnitRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateCanteenUnitCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.ComCode, ct)
            ?? throw new KeyNotFoundException($"Canteen unit {request.ComCode} not found.");

        entity.Update(request.UnitName, request.UnitRef, request.MaxVal, request.MinVal, request.SiteId, request.HrmsId);
        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
