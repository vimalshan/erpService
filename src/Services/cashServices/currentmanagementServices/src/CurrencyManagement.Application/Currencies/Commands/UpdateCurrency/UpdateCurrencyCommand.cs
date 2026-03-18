using AutoMapper;
using MediatR;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Domain.Interfaces;
using CurrencyManagement.Domain.ValueObjects;

namespace CurrencyManagement.Application.Currencies.Commands.UpdateCurrency;

/// <summary>
/// Command to update an existing currency
/// </summary>
public record UpdateCurrencyCommand(long CurrencyId, string Name, string Symbol, long ModifiedBy) : IRequest<CurrencyDto>;

/// <summary>
/// Handler for UpdateCurrencyCommand
/// </summary>
public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, CurrencyDto>
{
    private readonly ICurrencyRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCurrencyCommandHandler(ICurrencyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CurrencyDto> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = await _repository.GetByIdAsync(request.CurrencyId, cancellationToken);
        if (currency == null)
            throw new KeyNotFoundException($"Currency with ID {request.CurrencyId} not found");

        var symbol = CurrencySymbol.Create(request.Symbol);
        currency.Update(request.Name, symbol, request.ModifiedBy);

        await _repository.UpdateAsync(currency, cancellationToken);

        return _mapper.Map<CurrencyDto>(currency);
    }
}
