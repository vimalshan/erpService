using MediatR;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Domain.Interfaces;

namespace SSCTransactional.Application.Queries.Rescan;

public record GetRescansByDocIdQuery(long DocId) : IRequest<IEnumerable<RescanDto>>;
public record GetPendingRescansQuery() : IRequest<IEnumerable<RescanDto>>;

public class GetRescansByDocIdQueryHandler : IRequestHandler<GetRescansByDocIdQuery, IEnumerable<RescanDto>>
{
    private readonly IRescanRepository _repo;
    public GetRescansByDocIdQueryHandler(IRescanRepository repo) => _repo = repo;

    public async Task<IEnumerable<RescanDto>> Handle(GetRescansByDocIdQuery query, CancellationToken ct)
    {
        var list = await _repo.GetByDocIdAsync(query.DocId, ct);
        return list.Select(r => new RescanDto(r.Id, r.DocId, r.AllocationId, r.Status, r.RescanDate, r.RescanTo,
            r.RescanRemarks, r.CompletedOn, r.CompletedBy, r.CompletionRemarks, r.FilePath));
    }
}

public class GetPendingRescansQueryHandler : IRequestHandler<GetPendingRescansQuery, IEnumerable<RescanDto>>
{
    private readonly IRescanRepository _repo;
    public GetPendingRescansQueryHandler(IRescanRepository repo) => _repo = repo;

    public async Task<IEnumerable<RescanDto>> Handle(GetPendingRescansQuery query, CancellationToken ct)
    {
        var list = await _repo.GetPendingAsync(ct);
        return list.Select(r => new RescanDto(r.Id, r.DocId, r.AllocationId, r.Status, r.RescanDate, r.RescanTo,
            r.RescanRemarks, r.CompletedOn, r.CompletedBy, r.CompletionRemarks, r.FilePath));
    }
}
