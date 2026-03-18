using AutoMapper;
using MediatR;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Domain.Interfaces;

namespace CurrencyManagement.Application.ExchangeRates.Queries.GetExchangeRate;

/// <summary>
/// Query to get an exchange rate for a specific currency pair and period
/// </summary>
public record GetExchangeRateQuery(long FromCurrencyId, long ToCurrencyId, long FinancialYear, long Month) : IRequest<ExchangeRateDto>;

/// <summary>
/// Handler for GetExchangeRateQuery
/// </summary>
public class GetExchangeRateQueryHandler : IRequestHandler<GetExchangeRateQuery, ExchangeRateDto>
{
    private readonly IExchangeRateRepository _repository;
    private readonly IMapper _mapper;

    public GetExchangeRateQueryHandler(IExchangeRateRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ExchangeRateDto> Handle(GetExchangeRateQuery request, CancellationToken cancellationToken)
    {
        var exchangeRate = await _repository.GetRateAsync(request.FromCurrencyId, request.ToCurrencyId, request.FinancialYear, request.Month, cancellationToken);
        if (exchangeRate == null)
            throw new KeyNotFoundException($"Exchange rate not found for {request.FromCurrencyId} to {request.ToCurrencyId} in {request.FinancialYear}/{request.Month}");

        return _mapper.Map<ExchangeRateDto>(exchangeRate);
    }
}
