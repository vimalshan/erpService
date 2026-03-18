using MediatR;
using SettlementService.Domain.Interfaces;

namespace SettlementService.Application.Commands.AddPayment;

public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddPaymentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
    {
        var settlement = await _unitOfWork.Settlements.GetByIdAsync(request.SettlementNumber, cancellationToken)
            ?? throw new KeyNotFoundException($"Settlement {request.SettlementNumber} not found.");

        settlement.AddPayment(request.PaymentMode, request.Amount, request.ReferenceNo);
        await _unitOfWork.Settlements.UpdateAsync(settlement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
