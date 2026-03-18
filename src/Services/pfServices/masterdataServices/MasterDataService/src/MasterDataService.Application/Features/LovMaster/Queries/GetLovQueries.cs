using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Features.LovMaster.Queries;

public record GetAllLovQuery(string? Category = null) : IRequest<IEnumerable<LovMasterDto>>;
public record GetLovByIdQuery(decimal LovId) : IRequest<LovMasterDto?>;
public record GetLovByCategoryQuery(string Category) : IRequest<IEnumerable<LovMasterDto>>;
