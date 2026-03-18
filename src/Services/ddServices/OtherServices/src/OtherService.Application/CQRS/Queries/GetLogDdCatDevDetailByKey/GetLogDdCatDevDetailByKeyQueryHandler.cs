using AutoMapper;
using MediatR;
using OtherService.Application.DTOs;
using OtherService.Domain.Interfaces;

namespace OtherService.Application.CQRS.Queries.GetLogDdCatDevDetailByKey;

public sealed class GetLogDdCatDevDetailByKeyQueryHandler
    : IRequestHandler<GetLogDdCatDevDetailByKeyQuery, LogDdCatDevDetailDto?>
{
    private readonly ILogDdCatDevDetailRepository _repository;
    private readonly IMapper _mapper;

    public GetLogDdCatDevDetailByKeyQueryHandler(
        ILogDdCatDevDetailRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<LogDdCatDevDetailDto?> Handle(
        GetLogDdCatDevDetailByKeyQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByKeyAsync(request.AppId, request.AppNum, cancellationToken);
        return entity is null ? null : _mapper.Map<LogDdCatDevDetailDto>(entity);
    }
}
