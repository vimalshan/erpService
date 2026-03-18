using MediatR;
using VisitorServices.Application.Common.Interfaces;

namespace VisitorServices.Application.Visitors.Commands.CheckoutVisitor;

public sealed class CheckoutVisitorCommandHandler(
    IVisitorRepository visitorRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CheckoutVisitorCommand>
{
    public async Task Handle(CheckoutVisitorCommand request, CancellationToken cancellationToken)
    {
        var visitor = await visitorRepository.GetByIdAsync(request.VisitorId, cancellationToken)
            ?? throw new KeyNotFoundException($"Visitor {request.VisitorId} not found.");

        visitor.Checkout(request.CheckedOutBy);
        visitorRepository.Update(visitor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
