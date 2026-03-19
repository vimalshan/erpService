using InvoiceProcessing.Application.DTOs;
using MediatR;

namespace InvoiceProcessing.Application.Features.Documents.Commands;

public record CreateDocumentCommand(
    long Id,
    string OrgId,
    int LocationId,
    string DocumentType,
    long MainCategory,
    long SubCategory,
    string PoNumber,
    long VendorSiteId,
    long VendorId,
    int DueDays,
    long PoId,
    string InvoiceNo,
    long InvoiceAmount,
    int Currency,
    DateTime InvoiceDate,
    DateTime InvoiceReceiptDate,
    long Pages,
    DateTime PaymentDueDate,
    int PayBy,
    long Owner,
    string DocumentStatus,
    long UserId,
    string AccountCode
) : IRequest<DocumentDetailDto>;

public record UpdateDocumentCommand(
    long Id,
    string? Remarks,
    string? InvoiceStatus,
    string? HoldStatus,
    string? HoldPaymentRemarks,
    string? FilePath
) : IRequest<DocumentDetailDto>;

public record SubmitDocumentCommand(long Id) : IRequest<DocumentDetailDto>;

public record ApproveDocumentCommand(long Id, long ApprovedBy) : IRequest<DocumentDetailDto>;

public record CancelDocumentCommand(long Id, long CancelledBy, string? Remarks) : IRequest<DocumentDetailDto>;

public record HoldDocumentCommand(long Id, string? HoldRemarks) : IRequest<DocumentDetailDto>;

public record ReleaseHoldCommand(long Id, string? ReleaseRemarks) : IRequest<DocumentDetailDto>;

public record DeleteDocumentCommand(long Id) : IRequest<bool>;
