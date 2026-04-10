using AutoMapper;
using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Domain.Entities;
using CategoryAndVendorService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CategoryAndVendorService.Application.SupportDocuments.Commands;

// --- Create ---
public record CreateSupportDocumentCommand(
    long DocId, long DocCategory, long InvoiceDocId,
    string DocStatus, string? DocKey = null, string? PbgNo = null,
    DateTime? PbgStart = null, DateTime? PbgExpDate = null,
    long? Amount = null, long? RecDue = null) : IRequest<SupportDocumentDto>;

public class CreateSupportDocumentCommandValidator : AbstractValidator<CreateSupportDocumentCommand>
{
    public CreateSupportDocumentCommandValidator()
    {
        RuleFor(x => x.DocStatus).NotEmpty().MaximumLength(2);
    }
}

public class CreateSupportDocumentCommandHandler : IRequestHandler<CreateSupportDocumentCommand, SupportDocumentDto>
{
    private readonly ISupportDocumentRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateSupportDocumentCommandHandler(ISupportDocumentRepository repo, IUnitOfWork uow, IMapper mapper)
    { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<SupportDocumentDto> Handle(CreateSupportDocumentCommand request, CancellationToken ct)
    {
        var entity = SupportDocument.Create(request.DocId, request.DocCategory, request.InvoiceDocId,
            request.DocStatus, request.DocKey, request.PbgNo, request.PbgStart, request.PbgExpDate,
            request.Amount, request.RecDue);
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<SupportDocumentDto>(entity);
    }
}

// --- Delete ---
public record DeleteSupportDocumentCommand(long DocId) : IRequest<bool>;

public class DeleteSupportDocumentCommandHandler : IRequestHandler<DeleteSupportDocumentCommand, bool>
{
    private readonly ISupportDocumentRepository _repo;
    private readonly IUnitOfWork _uow;
    public DeleteSupportDocumentCommandHandler(ISupportDocumentRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<bool> Handle(DeleteSupportDocumentCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.DocId, ct);
        if (entity is null) return false;
        _repo.Delete(entity);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
