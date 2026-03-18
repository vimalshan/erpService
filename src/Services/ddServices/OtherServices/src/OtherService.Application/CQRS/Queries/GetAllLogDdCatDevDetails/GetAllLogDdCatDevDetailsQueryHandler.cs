using AutoMapper;
using MediatR;
using OtherService.Application.DTOs;
using OtherService.Domain.Interfaces;

namespace OtherService.Application.CQRS.Queries.GetAllLogDdCatDevDetails;

public sealed class GetAllLogDdCatDevDetailsQueryHandler
    : IRequestHandler<GetAllLogDdCatDevDetailsQuery, IEnumerable<LogDdCatDevDetailDto>>
{
    private readonly ILogDdCatDevDetailRepository _repository;
    private readonly IMapper _mapper;

    public GetAllLogDdCatDevDetailsQueryHandler(
        ILogDdCatDevDetailRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<IEnumerable<LogDdCatDevDetailDto>> Handle(
        GetAllLogDdCatDevDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<LogDdCatDevDetailDto>>(entities);
    }
}
