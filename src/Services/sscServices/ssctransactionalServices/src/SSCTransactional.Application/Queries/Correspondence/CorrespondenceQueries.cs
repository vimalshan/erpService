using MediatR;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Domain.Aggregates;
using SSCTransactional.Domain.Interfaces;

namespace SSCTransactional.Application.Queries.Correspondence;

public record GetCorrespondenceByIdQuery(long CorrespondenceId) : IRequest<CorrespondenceDto?>;
public record GetCorrespondencesByDocIdQuery(long DocId) : IRequest<IEnumerable<CorrespondenceDto>>;
public record GetActiveHoldsQuery() : IRequest<IEnumerable<CorrespondenceDto>>;

public class GetCorrespondenceByIdQueryHandler : IRequestHandler<GetCorrespondenceByIdQuery, CorrespondenceDto?>
{
    private readonly ICorrespondenceRepository _repo;
    public GetCorrespondenceByIdQueryHandler(ICorrespondenceRepository repo) => _repo = repo;

    public async Task<CorrespondenceDto?> Handle(GetCorrespondenceByIdQuery query, CancellationToken ct)
    {
        var corr = await _repo.GetByIdAsync(query.CorrespondenceId, ct);
        return corr is null ? null : MapToDto(corr);
    }

    private static CorrespondenceDto MapToDto(CorrespondenceAggregate c) => new(
        c.Id, c.DocId, c.AllocationId, c.HoldCategory, c.HoldType,
        c.HoldDate, c.HoldRemarks, c.HoldBy, c.HoldStatus,
        c.ReleaseDate, c.ReleaseRemarks, c.ReleasedBy, c.HoldNature,
        c.Attachments.Select(a => new CorrespondenceAttachmentDto(a.Id, a.CorrespondenceId, a.CorrespondenceStatus, a.FilePath)).ToList());
}

public class GetCorrespondencesByDocIdQueryHandler : IRequestHandler<GetCorrespondencesByDocIdQuery, IEnumerable<CorrespondenceDto>>
{
    private readonly ICorrespondenceRepository _repo;
    public GetCorrespondencesByDocIdQueryHandler(ICorrespondenceRepository repo) => _repo = repo;

    public async Task<IEnumerable<CorrespondenceDto>> Handle(GetCorrespondencesByDocIdQuery query, CancellationToken ct)
    {
        var list = await _repo.GetByDocIdAsync(query.DocId, ct);
        return list.Select(c => new CorrespondenceDto(
            c.Id, c.DocId, c.AllocationId, c.HoldCategory, c.HoldType,
            c.HoldDate, c.HoldRemarks, c.HoldBy, c.HoldStatus,
            c.ReleaseDate, c.ReleaseRemarks, c.ReleasedBy, c.HoldNature,
            c.Attachments.Select(a => new CorrespondenceAttachmentDto(a.Id, a.CorrespondenceId, a.CorrespondenceStatus, a.FilePath)).ToList()));
    }
}

public class GetActiveHoldsQueryHandler : IRequestHandler<GetActiveHoldsQuery, IEnumerable<CorrespondenceDto>>
{
    private readonly ICorrespondenceRepository _repo;
    public GetActiveHoldsQueryHandler(ICorrespondenceRepository repo) => _repo = repo;

    public async Task<IEnumerable<CorrespondenceDto>> Handle(GetActiveHoldsQuery query, CancellationToken ct)
    {
        var list = await _repo.GetActiveHoldsAsync(ct);
        return list.Select(c => new CorrespondenceDto(
            c.Id, c.DocId, c.AllocationId, c.HoldCategory, c.HoldType,
            c.HoldDate, c.HoldRemarks, c.HoldBy, c.HoldStatus,
            c.ReleaseDate, c.ReleaseRemarks, c.ReleasedBy, c.HoldNature));
    }
}
