namespace CategoryAndVendorService.Application.DTOs;

public record MainCategoryDto(
    long MainCatId,
    string MainCatName,
    long MainCatPriority,
    long ModifiedBy,
    DateTime ModifiedOn,
    long? DefaultSubCatId,
    long? AvgResponseTime);

public record SubCategoryDto(
    long SubCatId,
    long MainCatId,
    string SubCatName,
    long ModifiedBy,
    DateTime ModifiedOn);

public record VendorDocumentDto(
    long VndDocId,
    long VendorId,
    long SiteId,
    long BuId,
    long InformationCategory,
    string Remarks,
    string DocFlag,
    long? DocType,
    string? DocRefNo,
    DateTime ValidFrom,
    DateTime? ValidTo,
    string ActiveStatus,
    long ModifiedBy,
    DateTime ModifiedOn,
    string ApprovalStatusCode,
    string ApprovalStatusDescription,
    string? ApprovalRemarks,
    long? ApprovedBy,
    DateTime? ApprovedOn,
    List<VendorDocumentFileDto> Files);

public record VendorDocumentFileDto(
    long FileId,
    long DocumentId,
    string FileName,
    string? FilePath);

public record SupportDocumentDto(
    long DocId,
    long DocCategory,
    long InvoiceDocId,
    string? DocKey,
    string DocStatus,
    string? PbgNo,
    DateTime? PbgStart,
    DateTime? PbgExpDate,
    long? Amount,
    long? RecDue,
    List<SupportDocumentAttachmentDto> Attachments);

public record SupportDocumentAttachmentDto(
    long AttachmentId,
    long DocId,
    long InvoiceDocId,
    string RefFlag);
