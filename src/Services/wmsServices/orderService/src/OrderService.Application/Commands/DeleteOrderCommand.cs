using MediatR;

namespace OrderService.Application.Commands;

public record DeleteOrderCommand(int OrderId) : IRequest<Unit>;
