using AutoMapper;
using MediatR;
using MenuAndSecurityService.Application.DTOs;
using MenuAndSecurityService.Domain.Interfaces;

namespace MenuAndSecurityService.Application.Queries.GetMenuById;

public class GetMenuByIdQueryHandler : IRequestHandler<GetMenuByIdQuery, MenuMasterDto?>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMapper _mapper;

    public GetMenuByIdQueryHandler(IMenuRepository menuRepository, IMapper mapper)
    {
        _menuRepository = menuRepository;
        _mapper = mapper;
    }

    public async Task<MenuMasterDto?> Handle(GetMenuByIdQuery request, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByIdAsync(request.MenuId, cancellationToken);
        return menu is null ? null : _mapper.Map<MenuMasterDto>(menu);
    }
}
