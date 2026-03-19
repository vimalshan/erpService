using MediatR;
using MenuAndSecurityService.Domain.Interfaces;

namespace MenuAndSecurityService.Application.Commands.DeleteMenu;

public class DeleteMenuCommandHandler : IRequestHandler<DeleteMenuCommand, bool>
{
    private readonly IMenuRepository _menuRepository;

    public DeleteMenuCommandHandler(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<bool> Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByIdAsync(request.MenuId, cancellationToken);
        if (menu is null) return false;

        await _menuRepository.DeleteAsync(request.MenuId, cancellationToken);
        return true;
    }
}
