using MediatR;
using StipendService.Application.DTOs;

namespace StipendService.Application.Features.StipendMaster.Queries;

public record GetStipendMasterByIdQuery(long StipendId) : IRequest<StipendMasterDto?>;
public record GetAllStipendMastersQuery() : IRequest<IEnumerable<StipendMasterDto>>;
public record GetActiveStipendByCategoryQuery(long ResearchCategoryId, long SrfRankId) : IRequest<StipendMasterDto?>;
