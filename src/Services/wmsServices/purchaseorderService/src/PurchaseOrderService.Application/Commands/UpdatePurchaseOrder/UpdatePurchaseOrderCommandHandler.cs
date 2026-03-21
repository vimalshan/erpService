using MediatR;
using PurchaseOrderService.Domain.Interfaces;

namespace PurchaseOrderService.Application.Commands.UpdatePurchaseOrder;

public class UpdatePurchaseOrderCommandHandler : IRequestHandler<UpdatePurchaseOrderCommand, Unit>
{
    private readonly IPurchaseOrderRepository _repository;

    public UpdatePurchaseOrderCommandHandler(IPurchaseOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var po = await _repository.GetByIdAsync(request.PoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Purchase order with ID {request.PoId} not found.");

        po.Update(request.ExpectedDate, request.Notes);

        // Remove lines not in the update request
        var existingLineNumbers = po.Lines.Select(l => l.LineNumber).ToList();
        var requestLineNumbers = request.Lines.Select(l => l.LineNumber).ToHashSet();
        foreach (var lineNumber in existingLineNumbers.Where(ln => !requestLineNumbers.Contains(ln)))
        {
            po.RemoveLine(lineNumber);
        }

        // Update existing or add new lines
        foreach (var lineCmd in request.Lines)
        {
            var existingLine = po.Lines.FirstOrDefault(l => l.LineNumber == lineCmd.LineNumber);
            if (existingLine != null)
            {
                existingLine.Update(lineCmd.QuantityOrdered, lineCmd.UnitPrice, lineCmd.Notes);
            }
            else
            {
                po.AddLine(lineCmd.ProductId, lineCmd.LineNumber, lineCmd.QuantityOrdered, lineCmd.UnitPrice, lineCmd.Notes);
            }
        }

        await _repository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
