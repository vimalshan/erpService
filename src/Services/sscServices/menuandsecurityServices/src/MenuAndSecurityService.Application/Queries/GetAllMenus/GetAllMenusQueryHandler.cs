using AutoMapper;
using MediatR;
using MenuAndSecurityService.Application.DTOs;
using MenuAndSecurityService.Domain.Interfaces;

namespace MenuAndSecurityService.Application.Queries.GetAllMenus;

public class GetAllMenusQueryHandler : IRequestHandler<GetAllMenusQuery, IEnumerable<MenuMasterDto>>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMapper _mapper;

    public GetAllMenusQueryHandler(IMenuRepository menuRepository, IMapper mapper)
    {
        _menuRepository = menuRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MenuMasterDto>> Handle(GetAllMenusQuery request, CancellationToken cancellationToken)
    {
        var menus = await _menuRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<MenuMasterDto>>(menus);
    }
}
