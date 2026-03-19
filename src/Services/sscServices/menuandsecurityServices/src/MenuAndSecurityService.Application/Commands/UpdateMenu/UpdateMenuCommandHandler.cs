using AutoMapper;
using MediatR;
using MenuAndSecurityService.Application.DTOs;
using MenuAndSecurityService.Domain.Interfaces;

namespace MenuAndSecurityService.Application.Commands.UpdateMenu;

public class UpdateMenuCommandHandler : IRequestHandler<UpdateMenuCommand, MenuMasterDto>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMapper _mapper;

    public UpdateMenuCommandHandler(IMenuRepository menuRepository, IMapper mapper)
    {
        _menuRepository = menuRepository;
        _mapper = mapper;
    }

    public async Task<MenuMasterDto> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByIdAsync(request.MenuId, cancellationToken)
            ?? throw new KeyNotFoundException($"Menu with ID {request.MenuId} not found.");

        menu.Update(request.MenuName, request.MenuPageName, request.MenuParentId,
            request.MenuDisplayOrder, request.ModifiedBy);

        await _menuRepository.UpdateAsync(menu, cancellationToken);
        return _mapper.Map<MenuMasterDto>(menu);
    }
}
