using MediatR;
using SettlementService.Domain.Interfaces;

namespace SettlementService.Application.Commands.AddDeduction;

public class AddDeductionCommandHandler : IRequestHandler<AddDeductionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddDeductionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AddDeductionCommand request, CancellationToken cancellationToken)
    {
        var settlement = await _unitOfWork.Settlements.GetByIdAsync(request.SettlementNumber, cancellationToken)
            ?? throw new KeyNotFoundException($"Settlement {request.SettlementNumber} not found.");

        settlement.AddDeduction(request.DeductionType, request.Amount);
        await _unitOfWork.Settlements.UpdateAsync(settlement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
