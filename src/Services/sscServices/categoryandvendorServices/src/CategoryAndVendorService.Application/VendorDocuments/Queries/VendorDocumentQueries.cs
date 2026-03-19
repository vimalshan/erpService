using AutoMapper;
using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Domain.Interfaces;
using MediatR;

namespace CategoryAndVendorService.Application.VendorDocuments.Queries;

public record GetAllVendorDocumentsQuery : IRequest<IReadOnlyList<VendorDocumentDto>>;
public record GetVendorDocumentByIdQuery(long VndDocId) : IRequest<VendorDocumentDto?>;
public record GetVendorDocumentsByVendorIdQuery(long VendorId) : IRequest<IReadOnlyList<VendorDocumentDto>>;

public class GetAllVendorDocumentsQueryHandler : IRequestHandler<GetAllVendorDocumentsQuery, IReadOnlyList<VendorDocumentDto>>
{
    private readonly IVendorDocumentRepository _repo;
    private readonly IMapper _mapper;
    public GetAllVendorDocumentsQueryHandler(IVendorDocumentRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<VendorDocumentDto>> Handle(GetAllVendorDocumentsQuery request, CancellationToken ct)
    {
        var items = await _repo.GetAllAsync(ct);
        return items.Select(_mapper.Map<VendorDocumentDto>).ToList();
    }
}

public class GetVendorDocumentByIdQueryHandler : IRequestHandler<GetVendorDocumentByIdQuery, VendorDocumentDto?>
{
    private readonly IVendorDocumentRepository _repo;
    private readonly IMapper _mapper;
    public GetVendorDocumentByIdQueryHandler(IVendorDocumentRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<VendorDocumentDto?> Handle(GetVendorDocumentByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.VndDocId, ct);
        return entity is null ? null : _mapper.Map<VendorDocumentDto>(entity);
    }
}

public class GetVendorDocumentsByVendorIdQueryHandler : IRequestHandler<GetVendorDocumentsByVendorIdQuery, IReadOnlyList<VendorDocumentDto>>
{
    private readonly IVendorDocumentRepository _repo;
    private readonly IMapper _mapper;
    public GetVendorDocumentsByVendorIdQueryHandler(IVendorDocumentRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<VendorDocumentDto>> Handle(GetVendorDocumentsByVendorIdQuery request, CancellationToken ct)
    {
        var items = await _repo.GetByVendorIdAsync(request.VendorId, ct);
        return items.Select(_mapper.Map<VendorDocumentDto>).ToList();
    }
}
