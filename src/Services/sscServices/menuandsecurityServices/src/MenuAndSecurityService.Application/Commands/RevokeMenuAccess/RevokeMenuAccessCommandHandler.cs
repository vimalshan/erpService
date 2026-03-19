using MediatR;
using MenuAndSecurityService.Domain.Interfaces;

namespace MenuAndSecurityService.Application.Commands.RevokeMenuAccess;

public class RevokeMenuAccessCommandHandler : IRequestHandler<RevokeMenuAccessCommand, bool>
{
    private readonly IRoleMenuAccessRepository _repository;

    public RevokeMenuAccessCommandHandler(IRoleMenuAccessRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(RevokeMenuAccessCommand request, CancellationToken cancellationToken)
    {
        var access = await _repository.GetByIdAsync(request.MenuAccessId, cancellationToken);
        if (access is null) return false;

        access.Revoke();
        await _repository.DeleteAsync(request.MenuAccessId, cancellationToken);
        return true;
    }
}
