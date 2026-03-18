using AutoMapper;
using MediatR;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Domain.Interfaces;

namespace CurrencyManagement.Application.Currencies.Queries.GetCurrencyById;

/// <summary>
/// Query to get a currency by its ID
/// </summary>
public record GetCurrencyByIdQuery(long CurrencyId) : IRequest<CurrencyDto>;

/// <summary>
/// Handler for GetCurrencyByIdQuery
/// </summary>
public class GetCurrencyByIdQueryHandler : IRequestHandler<GetCurrencyByIdQuery, CurrencyDto>
{
    private readonly ICurrencyRepository _repository;
    private readonly IMapper _mapper;

    public GetCurrencyByIdQueryHandler(ICurrencyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CurrencyDto> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        var currency = await _repository.GetByIdAsync(request.CurrencyId, cancellationToken);
        if (currency == null)
            throw new KeyNotFoundException($"Currency with ID {request.CurrencyId} not found");

        return _mapper.Map<CurrencyDto>(currency);
    }
}
