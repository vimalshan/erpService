using AutoMapper;
using MediatR;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Domain.Interfaces;
using CurrencyManagement.Domain.ValueObjects;

namespace CurrencyManagement.Application.ExchangeRates.Commands.SetExchangeRate;

/// <summary>
/// Command to set/create an exchange rate
/// </summary>
public record SetExchangeRateCommand(
    long RateId,
    long FinancialYear,
    long Month,
    long FromCurrencyId,
    long ToCurrencyId,
    decimal Rate,
    long ModifiedBy) : IRequest<ExchangeRateDto>;

/// <summary>
/// Handler for SetExchangeRateCommand
/// </summary>
public class SetExchangeRateCommandHandler : IRequestHandler<SetExchangeRateCommand, ExchangeRateDto>
{
    private readonly IExchangeRateRepository _repository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IMapper _mapper;

    public SetExchangeRateCommandHandler(IExchangeRateRepository repository, ICurrencyRepository currencyRepository, IMapper mapper)
    {
        _repository = repository;
        _currencyRepository = currencyRepository;
        _mapper = mapper;
    }

    public async Task<ExchangeRateDto> Handle(SetExchangeRateCommand request, CancellationToken cancellationToken)
    {
        // Verify both currencies exist
        var fromCurrencyExists = await _currencyRepository.ExistsAsync(request.FromCurrencyId, cancellationToken);
        var toCurrencyExists = await _currencyRepository.ExistsAsync(request.ToCurrencyId, cancellationToken);

        if (!fromCurrencyExists)
            throw new KeyNotFoundException($"From Currency with ID {request.FromCurrencyId} not found");

        if (!toCurrencyExists)
            throw new KeyNotFoundException($"To Currency with ID {request.ToCurrencyId} not found");

        var rateValue = ExchangeRateValue.Create(request.Rate);
        var exchangeRate = new Domain.Entities.ExchangeRate(
            request.RateId,
            request.FinancialYear,
            request.Month,
            request.FromCurrencyId,
            request.ToCurrencyId,
            rateValue,
            request.ModifiedBy);

        // Check if rate already exists and update or add
        var existing = await _repository.GetRateAsync(request.FromCurrencyId, request.ToCurrencyId, request.FinancialYear, request.Month, cancellationToken);

        if (existing != null)
        {
            existing.UpdateRate(rateValue, request.ModifiedBy);
            await _repository.UpdateAsync(existing, cancellationToken);
            return _mapper.Map<ExchangeRateDto>(existing);
        }

        await _repository.AddAsync(exchangeRate, cancellationToken);
        return _mapper.Map<ExchangeRateDto>(exchangeRate);
    }
}
