using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Commands.Area;

public record CreateAreaCommand(int AreaId, string AreaName) : IRequest<AreaDto>;
public record UpdateAreaCommand(long Id, string AreaName) : IRequest<AreaDto>;
public record DeleteAreaCommand(long Id) : IRequest<bool>;
