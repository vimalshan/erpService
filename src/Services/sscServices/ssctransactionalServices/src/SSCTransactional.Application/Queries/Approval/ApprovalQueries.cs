using MediatR;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Domain.Interfaces;

namespace SSCTransactional.Application.Queries.Approval;

public record GetApprovalsByDocIdQuery(long DocId) : IRequest<IEnumerable<DocumentApprovalDto>>;

public class GetApprovalsByDocIdQueryHandler : IRequestHandler<GetApprovalsByDocIdQuery, IEnumerable<DocumentApprovalDto>>
{
    private readonly IDocumentApprovalRepository _repo;
    public GetApprovalsByDocIdQueryHandler(IDocumentApprovalRepository repo) => _repo = repo;

    public async Task<IEnumerable<DocumentApprovalDto>> Handle(GetApprovalsByDocIdQuery query, CancellationToken ct)
    {
        var list = await _repo.GetByDocIdAsync(query.DocId, ct);
        return list.Select(a => new DocumentApprovalDto(a.Id, a.DocId, a.ApproverUserId, a.Status, a.Remarks, a.ApprovalDate));
    }
}
