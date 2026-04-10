namespace SSCTransactional.Application.DTOs;

public record AllocationDto(
    long AllocationId, long DocId, string Action, long GroupId,
    string PullStatus, long PullUserId, int Priority,
    long AllocatedBy, DateTime AllocatedOn, string? Remarks,
    string ActionFlag, DateTime? ActionDate, long? CorrespondenceId,
    long? DefectType, string? CloseRemarks, long ModifiedBy, DateTime ModifiedOn,
    DateTime PulledOn, List<DefectiveAttachmentDto>? DefectiveAttachments = null);

public record DefectiveAttachmentDto(long AttachmentId, long AllocationId, string FilePath);

public record CorrespondenceDto(
    long CorrespondenceId, long DocId, long AllocationId,
    long HoldCategory, long HoldType, DateTime HoldDate,
    string HoldRemarks, long HoldBy, string HoldStatus,
    DateTime? ReleaseDate, string? ReleaseRemarks, long? ReleasedBy,
    decimal? HoldNature, List<CorrespondenceAttachmentDto>? Attachments = null);

public record CorrespondenceAttachmentDto(long AttachmentId, long CorrespondenceId, string Status, string FilePath);

public record DocumentApprovalDto(
    long ApprovalId, long DocId, long ApproverUserId,
    string Status, string? Remarks, DateTime ApprovalDate);

public record RescanDto(
    long RescanId, long DocId, long AllocationId,
    string Status, DateTime RescanDate, string RescanTo,
    string RescanRemarks, DateTime? CompletedOn, long? CompletedBy,
    string? CompletionRemarks, string? FilePath);

public record RevokeDto(
    long RevokeId, long DocId, string RevokeRemarks,
    string RevokeStatus, long RevokedBy, DateTime RevokedOn);

public record DocumentApproverDto(
    long ApproverId, string BusinessUnit, long LocationId,
    string ApproverType, long ApproverEmpId, long EnteredBy, DateTime EnteredOn);

public record OracleInvoiceDto(
    long InvoiceDetId, long DocId, decimal? VoucherNo,
    string? InvoiceType, long? VendorId, long? VendorSiteId,
    string? InvoiceNum, DateTime? InvoiceDate, decimal? InvoiceAmount,
    long InvoiceId, string? InvoiceStatus, string? PaymentMethodCode, DateTime? AccountingDate);

public record OraclePaymentDto(
    long PaymentDetId, long DocId, long PaymentNum,
    long InvoiceId, DateTime? DueDate, decimal? GrossAmount,
    decimal? AmountRemaining, string? PaymentStatus, string? PaymentMethod,
    long? CheckId, string? BankStatus, long? CheckNumber, DateTime? CheckDate, decimal? CheckAmount);

public record OracleBankDetailDto(
    long BankDetId, long DocId, string CheckId,
    string? Business, string? OrgId, string? Amount,
    string? Currency, string? PaymentNumber, string? StatusLookupCode);

public record OracleDueDetailDto(
    long DueId, long DocId, long? OrgId, long InvoiceId,
    decimal? VoucherNo, string? DocumentId, DateTime? DueDate,
    long? PaymentNum, decimal? DueAmount);

public record DocumentStatusDto(
    string Flag, string DocType, string CompletedRemark,
    string PendingRemark, long? StageOrder, string? CategoryGroup, long? StageNo);
