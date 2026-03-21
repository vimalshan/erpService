using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Queries.TaxSlab;

public record GetAllTaxSlabsQuery : IRequest<IReadOnlyList<TaxSlabDto>>;
public record GetActiveTaxSlabsQuery : IRequest<IReadOnlyList<TaxSlabDto>>;
