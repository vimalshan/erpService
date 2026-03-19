using AutoMapper;
using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Domain.Interfaces;
using MediatR;

namespace CategoryAndVendorService.Application.SupportDocuments.Queries;

public record GetAllSupportDocumentsQuery : IRequest<IReadOnlyList<SupportDocumentDto>>;
public record GetSupportDocumentByIdQuery(long DocId) : IRequest<SupportDocumentDto?>;

public class GetAllSupportDocumentsQueryHandler : IRequestHandler<GetAllSupportDocumentsQuery, IReadOnlyList<SupportDocumentDto>>
{
    private readonly ISupportDocumentRepository _repo;
    private readonly IMapper _mapper;
    public GetAllSupportDocumentsQueryHandler(ISupportDocumentRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<SupportDocumentDto>> Handle(GetAllSupportDocumentsQuery request, CancellationToken ct)
    {
        var items = await _repo.GetAllAsync(ct);
        return items.Select(_mapper.Map<SupportDocumentDto>).ToList();
    }
}

public class GetSupportDocumentByIdQueryHandler : IRequestHandler<GetSupportDocumentByIdQuery, SupportDocumentDto?>
{
    private readonly ISupportDocumentRepository _repo;
    private readonly IMapper _mapper;
    public GetSupportDocumentByIdQueryHandler(ISupportDocumentRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<SupportDocumentDto?> Handle(GetSupportDocumentByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.DocId, ct);
        return entity is null ? null : _mapper.Map<SupportDocumentDto>(entity);
    }
}
