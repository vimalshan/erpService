using MediatR;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Commands.Handlers;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Unit>
{
    private readonly IOrderRepository _repository;

    public UpdateOrderStatusCommandHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order {request.OrderId} not found.");

        switch (request.Status.ToUpperInvariant())
        {
            case "PROCESSING": order.Process(); break;
            case "SHIPPED": order.Ship(); break;
            case "CANCELLED": order.Cancel(); break;
            default: throw new ArgumentException($"Invalid status: {request.Status}");
        }

        await _repository.UpdateAsync(order, cancellationToken);
        return Unit.Value;
    }
}
