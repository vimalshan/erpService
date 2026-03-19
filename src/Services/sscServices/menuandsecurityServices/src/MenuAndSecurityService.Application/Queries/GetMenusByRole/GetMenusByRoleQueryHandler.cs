using AutoMapper;
using MediatR;
using MenuAndSecurityService.Application.DTOs;
using MenuAndSecurityService.Domain.Interfaces;

namespace MenuAndSecurityService.Application.Queries.GetMenusByRole;

public class GetMenusByRoleQueryHandler : IRequestHandler<GetMenusByRoleQuery, IEnumerable<RoleMenuAccessDto>>
{
    private readonly IRoleMenuAccessRepository _repository;
    private readonly IMapper _mapper;

    public GetMenusByRoleQueryHandler(IRoleMenuAccessRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleMenuAccessDto>> Handle(GetMenusByRoleQuery request, CancellationToken cancellationToken)
    {
        var accesses = await _repository.GetByRoleIdAsync(request.RoleId, cancellationToken);
        return _mapper.Map<IEnumerable<RoleMenuAccessDto>>(accesses);
    }
}
