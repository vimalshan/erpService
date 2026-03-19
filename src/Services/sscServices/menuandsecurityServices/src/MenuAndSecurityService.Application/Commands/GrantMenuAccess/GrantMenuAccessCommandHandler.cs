using AutoMapper;
using MediatR;
using MenuAndSecurityService.Application.DTOs;
using MenuAndSecurityService.Domain.Entities;
using MenuAndSecurityService.Domain.Interfaces;

namespace MenuAndSecurityService.Application.Commands.GrantMenuAccess;

public class GrantMenuAccessCommandHandler : IRequestHandler<GrantMenuAccessCommand, RoleMenuAccessDto>
{
    private readonly IRoleMenuAccessRepository _repository;
    private readonly IMapper _mapper;

    public GrantMenuAccessCommandHandler(IRoleMenuAccessRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RoleMenuAccessDto> Handle(GrantMenuAccessCommand request, CancellationToken cancellationToken)
    {
        var access = RoleMenuAccess.Grant(request.MenuAccessId, request.MenuId, request.MenuRoleId, request.ModifiedBy);
        var created = await _repository.AddAsync(access, cancellationToken);
        return _mapper.Map<RoleMenuAccessDto>(created);
    }
}
