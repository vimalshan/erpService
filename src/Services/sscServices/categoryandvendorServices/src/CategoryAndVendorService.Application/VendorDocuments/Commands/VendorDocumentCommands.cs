using AutoMapper;
using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Domain.Entities;
using CategoryAndVendorService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CategoryAndVendorService.Application.VendorDocuments.Commands;

// --- Create ---
public record CreateVendorDocumentCommand(
    long VndDocId, long VendorId, long SiteId, long BuId,
    long InformationCategory, string Remarks, string DocFlag,
    DateTime ValidFrom, long ModifiedBy, long? DocType = null,
    string? DocRefNo = null, DateTime? ValidTo = null) : IRequest<VendorDocumentDto>;

public class CreateVendorDocumentCommandValidator : AbstractValidator<CreateVendorDocumentCommand>
{
    public CreateVendorDocumentCommandValidator()
    {
        RuleFor(x => x.Remarks).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.VendorId).GreaterThan(0);
    }
}

public class CreateVendorDocumentCommandHandler : IRequestHandler<CreateVendorDocumentCommand, VendorDocumentDto>
{
    private readonly IVendorDocumentRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateVendorDocumentCommandHandler(IVendorDocumentRepository repo, IUnitOfWork uow, IMapper mapper)
    { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<VendorDocumentDto> Handle(CreateVendorDocumentCommand request, CancellationToken ct)
    {
        var entity = VendorDocument.Create(
            request.VndDocId, request.VendorId, request.SiteId, request.BuId,
            request.InformationCategory, request.Remarks, request.DocFlag[0],
            request.ValidFrom, request.ModifiedBy, request.DocType,
            request.DocRefNo, request.ValidTo);
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<VendorDocumentDto>(entity);
    }
}

// --- Approve ---
public record ApproveVendorDocumentCommand(long VndDocId, long ApprovedBy, string? Remarks) : IRequest<VendorDocumentDto>;

public class ApproveVendorDocumentCommandHandler : IRequestHandler<ApproveVendorDocumentCommand, VendorDocumentDto>
{
    private readonly IVendorDocumentRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ApproveVendorDocumentCommandHandler(IVendorDocumentRepository repo, IUnitOfWork uow, IMapper mapper)
    { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<VendorDocumentDto> Handle(ApproveVendorDocumentCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.VndDocId, ct)
            ?? throw new KeyNotFoundException($"VendorDocument {request.VndDocId} not found.");
        entity.Approve(request.ApprovedBy, request.Remarks);
        _repo.Update(entity);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<VendorDocumentDto>(entity);
    }
}

// --- Reject ---
public record RejectVendorDocumentCommand(long VndDocId, long RejectedBy, string Remarks) : IRequest<VendorDocumentDto>;

public class RejectVendorDocumentCommandHandler : IRequestHandler<RejectVendorDocumentCommand, VendorDocumentDto>
{
    private readonly IVendorDocumentRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RejectVendorDocumentCommandHandler(IVendorDocumentRepository repo, IUnitOfWork uow, IMapper mapper)
    { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<VendorDocumentDto> Handle(RejectVendorDocumentCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.VndDocId, ct)
            ?? throw new KeyNotFoundException($"VendorDocument {request.VndDocId} not found.");
        entity.Reject(request.RejectedBy, request.Remarks);
        _repo.Update(entity);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<VendorDocumentDto>(entity);
    }
}

// --- Delete ---
public record DeleteVendorDocumentCommand(long VndDocId) : IRequest<bool>;

public class DeleteVendorDocumentCommandHandler : IRequestHandler<DeleteVendorDocumentCommand, bool>
{
    private readonly IVendorDocumentRepository _repo;
    private readonly IUnitOfWork _uow;
    public DeleteVendorDocumentCommandHandler(IVendorDocumentRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<bool> Handle(DeleteVendorDocumentCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.VndDocId, ct);
        if (entity is null) return false;
        _repo.Delete(entity);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
