using AutoMapper;
using MediatR;
using ReceivingService.Application.DTOs;
using ReceivingService.Domain.Interfaces;

namespace ReceivingService.Application.Queries.GetAllReceivings;

public sealed class GetAllReceivingsQueryHandler
    : IRequestHandler<GetAllReceivingsQuery, IEnumerable<ReceivingDto>>
{
    private readonly IReceivingRepository _repository;
    private readonly IMapper _mapper;

    public GetAllReceivingsQueryHandler(IReceivingRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<IEnumerable<ReceivingDto>> Handle(
        GetAllReceivingsQuery request, CancellationToken cancellationToken)
    {
        var receivings = await _repository.GetAllAsync(request.Page, request.PageSize, cancellationToken);
        return _mapper.Map<IEnumerable<ReceivingDto>>(receivings);
    }
}
