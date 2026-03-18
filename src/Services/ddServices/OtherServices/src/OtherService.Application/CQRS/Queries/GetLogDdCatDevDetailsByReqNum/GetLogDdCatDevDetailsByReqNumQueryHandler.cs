using AutoMapper;
using MediatR;
using OtherService.Application.DTOs;
using OtherService.Domain.Interfaces;

namespace OtherService.Application.CQRS.Queries.GetLogDdCatDevDetailsByReqNum;

public sealed class GetLogDdCatDevDetailsByReqNumQueryHandler
    : IRequestHandler<GetLogDdCatDevDetailsByReqNumQuery, IEnumerable<LogDdCatDevDetailDto>>
{
    private readonly ILogDdCatDevDetailRepository _repository;
    private readonly IMapper _mapper;

    public GetLogDdCatDevDetailsByReqNumQueryHandler(
        ILogDdCatDevDetailRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<IEnumerable<LogDdCatDevDetailDto>> Handle(
        GetLogDdCatDevDetailsByReqNumQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByReqNumAsync(request.ReqNum, cancellationToken);
        return _mapper.Map<IEnumerable<LogDdCatDevDetailDto>>(entities);
    }
}
