using AutoMapper;
using MediatR;
using ReceivingService.Application.DTOs;
using ReceivingService.Domain.Interfaces;

namespace ReceivingService.Application.Queries.GetReceivingById;

public sealed class GetReceivingByIdQueryHandler
    : IRequestHandler<GetReceivingByIdQuery, ReceivingDto>
{
    private readonly IReceivingRepository _repository;
    private readonly IMapper _mapper;

    public GetReceivingByIdQueryHandler(IReceivingRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<ReceivingDto> Handle(
        GetReceivingByIdQuery request, CancellationToken cancellationToken)
    {
        var receiving = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Receiving with id {request.Id} was not found.");

        return _mapper.Map<ReceivingDto>(receiving);
    }
}
