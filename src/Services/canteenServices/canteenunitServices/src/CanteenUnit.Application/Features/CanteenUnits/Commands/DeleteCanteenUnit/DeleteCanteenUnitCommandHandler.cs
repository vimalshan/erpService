using CanteenUnit.Domain.Interfaces;
using MediatR;

namespace CanteenUnit.Application.Features.CanteenUnits.Commands.DeleteCanteenUnit;

public class DeleteCanteenUnitCommandHandler : IRequestHandler<DeleteCanteenUnitCommand>
{
    private readonly ICanteenUnitRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCanteenUnitCommandHandler(ICanteenUnitRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCanteenUnitCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.ComCode, ct)
            ?? throw new KeyNotFoundException($"Canteen unit {request.ComCode} not found.");

        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
