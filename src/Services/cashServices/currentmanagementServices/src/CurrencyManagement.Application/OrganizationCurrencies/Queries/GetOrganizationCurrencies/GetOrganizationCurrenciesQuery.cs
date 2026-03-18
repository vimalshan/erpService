using AutoMapper;
using MediatR;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Domain.Interfaces;

namespace CurrencyManagement.Application.OrganizationCurrencies.Queries.GetOrganizationCurrencies;

/// <summary>
/// Query to get all currencies mapped to an organization
/// </summary>
public record GetOrganizationCurrenciesQuery(long OrganizationId) : IRequest<IList<OrganizationCurrencyDto>>;

/// <summary>
/// Handler for GetOrganizationCurrenciesQuery
/// </summary>
public class GetOrganizationCurrenciesQueryHandler : IRequestHandler<GetOrganizationCurrenciesQuery, IList<OrganizationCurrencyDto>>
{
    private readonly IOrganizationCurrencyRepository _repository;
    private readonly IMapper _mapper;

    public GetOrganizationCurrenciesQueryHandler(IOrganizationCurrencyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IList<OrganizationCurrencyDto>> Handle(GetOrganizationCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var mappings = await _repository.GetByOrganizationAsync(request.OrganizationId, cancellationToken);
        return _mapper.Map<IList<OrganizationCurrencyDto>>(mappings);
    }
}
