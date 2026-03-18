using AutoMapper;
using MediatR;
using CardManagement.Application.Common.DTOs;
using CardManagement.Domain.Interfaces;

namespace CardManagement.Application.Cards.Queries.GetGuestCardById;

public class GetGuestCardByIdQueryHandler : IRequestHandler<GetGuestCardByIdQuery, GuestCardDto?>
{
    private readonly IGuestCardMasterRepository _repository;
    private readonly IMapper _mapper;

    public GetGuestCardByIdQueryHandler(IGuestCardMasterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GuestCardDto?> Handle(GetGuestCardByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.CanteenUnit, cancellationToken);
        return entity is null ? null : _mapper.Map<GuestCardDto>(entity);
    }
}
