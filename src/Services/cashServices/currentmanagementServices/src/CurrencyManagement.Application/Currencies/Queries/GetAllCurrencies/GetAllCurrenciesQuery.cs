using AutoMapper;
using MediatR;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Domain.Interfaces;

namespace CurrencyManagement.Application.Currencies.Queries.GetAllCurrencies;

/// <summary>
/// Query to get all currencies
/// </summary>
public record GetAllCurrenciesQuery : IRequest<IList<CurrencyDto>>;

/// <summary>
/// Handler for GetAllCurrenciesQuery
/// </summary>
public class GetAllCurrenciesQueryHandler : IRequestHandler<GetAllCurrenciesQuery, IList<CurrencyDto>>
{
    private readonly ICurrencyRepository _repository;
    private readonly IMapper _mapper;

    public GetAllCurrenciesQueryHandler(ICurrencyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IList<CurrencyDto>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var currencies = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IList<CurrencyDto>>(currencies);
    }
}
