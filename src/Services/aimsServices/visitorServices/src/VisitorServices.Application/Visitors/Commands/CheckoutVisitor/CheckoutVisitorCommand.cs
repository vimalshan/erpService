using MediatR;

namespace VisitorServices.Application.Visitors.Commands.CheckoutVisitor;

public sealed record CheckoutVisitorCommand(long VisitorId, long CheckedOutBy) : IRequest;
