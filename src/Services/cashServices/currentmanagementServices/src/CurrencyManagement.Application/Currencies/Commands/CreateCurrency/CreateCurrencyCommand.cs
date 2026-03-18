using AutoMapper;
using MediatR;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Domain.Interfaces;
using CurrencyManagement.Domain.ValueObjects;

namespace CurrencyManagement.Application.Currencies.Commands.CreateCurrency;

/// <summary>
/// Command to create a new currency
/// </summary>
public record CreateCurrencyCommand(long CurrencyId, string Name, string Symbol, long ModifiedBy) : IRequest<CurrencyDto>;

/// <summary>
/// Handler for CreateCurrencyCommand
/// </summary>
public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, CurrencyDto>
{
    private readonly ICurrencyRepository _repository;
    private readonly IMapper _mapper;

    public CreateCurrencyCommandHandler(ICurrencyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CurrencyDto> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var existingCurrency = await _repository.GetByIdAsync(request.CurrencyId, cancellationToken);
        if (existingCurrency != null)
            throw new InvalidOperationException($"Currency with ID {request.CurrencyId} already exists");

        var symbol = CurrencySymbol.Create(request.Symbol);
        var currency = new Domain.Entities.Currency(request.CurrencyId, request.Name, symbol, request.ModifiedBy);

        await _repository.AddAsync(currency, cancellationToken);

        return _mapper.Map<CurrencyDto>(currency);
    }
}
