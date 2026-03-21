using MediatR;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Commands.Handlers;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IOrderRepository _repository;

    public DeleteOrderCommandHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.OrderId, cancellationToken);
        return Unit.Value;
    }
}
