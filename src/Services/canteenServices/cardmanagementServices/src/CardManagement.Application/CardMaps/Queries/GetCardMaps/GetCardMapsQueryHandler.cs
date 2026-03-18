using AutoMapper;
using MediatR;
using CardManagement.Application.Common.DTOs;
using CardManagement.Domain.Interfaces;

namespace CardManagement.Application.CardMaps.Queries.GetCardMaps;

public class GetCardMapsQueryHandler : IRequestHandler<GetCardMapsQuery, IEnumerable<CanteenCardMapDto>>
{
    private readonly ICanteenCardMapRepository _repository;
    private readonly IMapper _mapper;

    public GetCardMapsQueryHandler(ICanteenCardMapRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CanteenCardMapDto>> Handle(GetCardMapsQuery request, CancellationToken cancellationToken)
    {
        var entities = request.ActiveOnly
            ? await _repository.GetActiveByCanteenUnitAsync(request.CanteenUnit, cancellationToken)
            : await _repository.GetByCanteenUnitAsync(request.CanteenUnit, cancellationToken);

        return _mapper.Map<IEnumerable<CanteenCardMapDto>>(entities);
    }
}
