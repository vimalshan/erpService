using AutoMapper;
using MediatR;
using BatchService.Application.DTOs;
using BatchService.Domain.Interfaces;

namespace BatchService.Application.Queries.GetBatchesByMonth;

public sealed class GetBatchesByMonthQueryHandler : IRequestHandler<GetBatchesByMonthQuery, IEnumerable<BatchDto>>
{
    private readonly IBatchRepository _repository;
    private readonly IMapper          _mapper;

    public GetBatchesByMonthQueryHandler(IBatchRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<IEnumerable<BatchDto>> Handle(GetBatchesByMonthQuery req, CancellationToken ct)
    {
        var batches = await _repository.GetByMonthAsync(req.MonthNo, ct);
        return _mapper.Map<IEnumerable<BatchDto>>(batches);
    }
}
