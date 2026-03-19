using MediatR;
using MenuAndSecurityService.Application.DTOs;

namespace MenuAndSecurityService.Application.Queries.GetAllMenus;

public sealed record GetAllMenusQuery : IRequest<IEnumerable<MenuMasterDto>>;
