using MediatR;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Domain.Interfaces;

namespace CurrencyManagement.Application.ExchangeRates.Queries.ConvertAmount;

/// <summary>
/// Query to convert an amount from one currency to another
/// </summary>
public record ConvertAmountQuery(long FromCurrencyId, long ToCurrencyId, decimal Amount, long FinancialYear, long Month) : IRequest<ConvertedAmountDto>;

/// <summary>
/// Handler for ConvertAmountQuery
/// </summary>
public class ConvertAmountQueryHandler : IRequestHandler<ConvertAmountQuery, ConvertedAmountDto>
{
    private readonly IExchangeRateRepository _repository;

    public ConvertAmountQueryHandler(IExchangeRateRepository repository)
    {
        _repository = repository;
    }

    public async Task<ConvertedAmountDto> Handle(ConvertAmountQuery request, CancellationToken cancellationToken)
    {
        if (request.Amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(request.Amount));

        var exchangeRate = await _repository.GetRateAsync(request.FromCurrencyId, request.ToCurrencyId, request.FinancialYear, request.Month, cancellationToken);
        if (exchangeRate == null)
            throw new KeyNotFoundException($"Exchange rate not found for conversion from {request.FromCurrencyId} to {request.ToCurrencyId}");

        var convertedAmount = exchangeRate.ConvertAmount(request.Amount);

        return new ConvertedAmountDto
        {
            OriginalAmount = request.Amount,
            FromCurrencyId = request.FromCurrencyId,
            ToCurrencyId = request.ToCurrencyId,
            ExchangeRate = exchangeRate.Rate.Value,
            ConvertedAmount = convertedAmount,
            FinancialYear = request.FinancialYear,
            Month = request.Month
        };
    }
}
