using AutoMapper;
using ExpenseService.Application.Commands;
using ExpenseService.Application.DTOs;
using ExpenseService.Domain.Entities;
using ExpenseService.Domain.Interfaces;
using MediatR;

namespace ExpenseService.Application.Handlers;

public class CreateCurrencyRequestHandler : IRequestHandler<CreateCurrencyRequestCommand, CurrencyDto>
{
    private readonly ICurrencyRepository _repository;
    private readonly IMapper _mapper;

    public CreateCurrencyRequestHandler(ICurrencyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CurrencyDto> Handle(CreateCurrencyRequestCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByRequestNumberAsync(request.RequestNumber, cancellationToken);
        var nextSerial = existing.Count > 0 ? existing.Max(c => c.SerialNumber) + 1 : 1;

        var currency = new TravelCurrency
        {
            RequestNumber = request.RequestNumber,
            SerialNumber = nextSerial,
            CurrencyCode = request.CurrencyCode,
            CashAmount = request.CashAmount,
            TravellerChequeAmount = request.TravellerChequeAmount,
            DenominationFlag = request.DenominationFlag,
            DenominationText = request.DenominationText
        };

        var created = await _repository.AddAsync(currency, cancellationToken);
        return _mapper.Map<CurrencyDto>(created);
    }
}
