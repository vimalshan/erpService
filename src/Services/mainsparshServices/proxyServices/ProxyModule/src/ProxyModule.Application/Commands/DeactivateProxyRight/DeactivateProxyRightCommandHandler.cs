using MediatR;
using ProxyModule.Domain.Exceptions;
using ProxyModule.Domain.Interfaces;

namespace ProxyModule.Application.Commands.DeactivateProxyRight;

public sealed class DeactivateProxyRightCommandHandler : IRequestHandler<DeactivateProxyRightCommand, bool>
{
    private readonly IProxyRightRepository _repository;

    public DeactivateProxyRightCommandHandler(IProxyRightRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeactivateProxyRightCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.ProxyId, cancellationToken)
            ?? throw new ProxyDomainException($"Proxy right with ID {request.ProxyId} not found.");

        entity.Deactivate(request.UpdatedBy);
        await _repository.UpdateAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
