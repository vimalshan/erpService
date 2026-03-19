using AutoMapper;
using MediatR;
using MenuAndSecurityService.Application.DTOs;
using MenuAndSecurityService.Domain.Entities;
using MenuAndSecurityService.Domain.Interfaces;

namespace MenuAndSecurityService.Application.Commands.CreateMenu;

public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, MenuMasterDto>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMapper _mapper;

    public CreateMenuCommandHandler(IMenuRepository menuRepository, IMapper mapper)
    {
        _menuRepository = menuRepository;
        _mapper = mapper;
    }

    public async Task<MenuMasterDto> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        var menu = MenuMaster.Create(
            request.MenuId,
            request.MenuName,
            request.MenuPageName,
            request.MenuParentId,
            request.MenuDisplayOrder,
            request.ModifiedBy);

        var created = await _menuRepository.AddAsync(menu, cancellationToken);
        return _mapper.Map<MenuMasterDto>(created);
    }
}
