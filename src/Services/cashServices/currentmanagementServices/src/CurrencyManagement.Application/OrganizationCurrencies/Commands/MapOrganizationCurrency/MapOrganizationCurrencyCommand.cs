using AutoMapper;
using MediatR;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Domain.Interfaces;

namespace CurrencyManagement.Application.OrganizationCurrencies.Commands.MapOrganizationCurrency;

/// <summary>
/// Command to map an organization to a currency
/// </summary>
public record MapOrganizationCurrencyCommand(long OrganizationId, long CurrencyId, long ModifiedBy) : IRequest<OrganizationCurrencyDto>;

/// <summary>
/// Handler for MapOrganizationCurrencyCommand
/// </summary>
public class MapOrganizationCurrencyCommandHandler : IRequestHandler<MapOrganizationCurrencyCommand, OrganizationCurrencyDto>
{
    private readonly IOrganizationCurrencyRepository _repository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IMapper _mapper;

    public MapOrganizationCurrencyCommandHandler(
        IOrganizationCurrencyRepository repository,
        ICurrencyRepository currencyRepository,
        IMapper mapper)
    {
        _repository = repository;
        _currencyRepository = currencyRepository;
        _mapper = mapper;
    }

    public async Task<OrganizationCurrencyDto> Handle(MapOrganizationCurrencyCommand request, CancellationToken cancellationToken)
    {
        // Verify currency exists
        var currencyExists = await _currencyRepository.ExistsAsync(request.CurrencyId, cancellationToken);
        if (!currencyExists)
            throw new KeyNotFoundException($"Currency with ID {request.CurrencyId} not found");

        var mapping = new Domain.Entities.OrganizationCurrencyMapping(request.OrganizationId, request.CurrencyId, request.ModifiedBy);
        await _repository.AddAsync(mapping, cancellationToken);

        return _mapper.Map<OrganizationCurrencyDto>(mapping);
    }
}
