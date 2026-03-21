using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Queries.Area;

public record GetAllAreasQuery : IRequest<IReadOnlyList<AreaDto>>;
public record GetAreaByIdQuery(long Id) : IRequest<AreaDto?>;
