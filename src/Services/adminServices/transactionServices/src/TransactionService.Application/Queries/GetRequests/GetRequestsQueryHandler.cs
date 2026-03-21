namespace TransactionService.Application.Queries.GetRequests;

using AutoMapper;
using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Interfaces;

public sealed class GetAllRequestsQueryHandler : IRequestHandler<GetAllRequestsQuery, IEnumerable<RequestSummaryDto>>
{
    private readonly IRequestRepository _repository;
    private readonly IMapper _mapper;

    public GetAllRequestsQueryHandler(IRequestRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RequestSummaryDto>> Handle(
        GetAllRequestsQuery request, CancellationToken cancellationToken)
    {
        var requests = request.LocationId.HasValue
            ? await _repository.GetByLocationAsync(request.LocationId.Value, cancellationToken)
            : await _repository.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<RequestSummaryDto>>(requests);
    }
}

public sealed class GetRequestByIdQueryHandler : IRequestHandler<GetRequestByIdQuery, RequestMainDto?>
{
    private readonly IRequestRepository _repository;
    private readonly IMapper _mapper;

    public GetRequestByIdQueryHandler(IRequestRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RequestMainDto?> Handle(GetRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var requestMain = await _repository.GetByIdWithDetailsAsync(request.RequestId, cancellationToken);
        return requestMain is null ? null : _mapper.Map<RequestMainDto>(requestMain);
    }
}

public sealed class GetRequestsByEmployeeQueryHandler : IRequestHandler<GetRequestsByEmployeeQuery, IEnumerable<RequestSummaryDto>>
{
    private readonly IRequestRepository _repository;
    private readonly IMapper _mapper;

    public GetRequestsByEmployeeQueryHandler(IRequestRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RequestSummaryDto>> Handle(
        GetRequestsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var requests = await _repository.GetByEmployeeAsync(request.EmpSysId, cancellationToken);
        return _mapper.Map<IEnumerable<RequestSummaryDto>>(requests);
    }
}
