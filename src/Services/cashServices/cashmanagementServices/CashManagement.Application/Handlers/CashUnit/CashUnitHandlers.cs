using MediatR;
using CashManagement.Application.Commands.CashUnit;
using CashManagement.Application.DTOs;
using CashManagement.Domain.Entities;
using CashManagement.Domain.Interfaces;
using CashManagement.Domain.Interfaces.Repositories;

namespace CashManagement.Application.Handlers.CashUnit;

public class CreateCashUnitHandler : IRequestHandler<CreateCashUnitCommand, CashUnitDto>
{
    private readonly ICashUnitRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCashUnitHandler(ICashUnitRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CashUnitDto> Handle(CreateCashUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = Domain.Entities.CashUnit.Create(
            request.CashUnitId, request.Name, request.Code,
            request.Location, request.InChargeEmployeeId,
            request.OpeningBalance, request.CreatedBy);

        await _repository.AddAsync(unit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CashUnitDto(unit.Id, unit.Name, unit.Code, unit.Location,
            unit.InChargeEmployeeId, unit.OpeningBalance, unit.Status.ToString(),
            unit.OpeningBalance, unit.CreatedOn);
    }
}

public class UpdateCashUnitStatusHandler : IRequestHandler<UpdateCashUnitStatusCommand, bool>
{
    private readonly ICashUnitRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCashUnitStatusHandler(ICashUnitRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateCashUnitStatusCommand request, CancellationToken cancellationToken)
    {
        var unit = await _repository.GetByIdAsync(request.CashUnitId, cancellationToken);
        if (unit is null) return false;

        if (request.IsActive) unit.Activate(request.UpdatedBy);
        else unit.Deactivate(request.UpdatedBy);

        await _repository.UpdateAsync(unit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
