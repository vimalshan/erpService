using MediatR;

namespace OrderService.Application.Commands;

public record UpdateOrderStatusCommand(int OrderId, string Status) : IRequest<Unit>;
