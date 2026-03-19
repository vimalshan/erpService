using MediatR;
using MenuAndSecurityService.Application.DTOs;

namespace MenuAndSecurityService.Application.Queries.GetMenuById;

public sealed record GetMenuByIdQuery(long MenuId) : IRequest<MenuMasterDto?>;
