namespace InvoiceProcessing.Application.DTOs;

public class DocumentDetailDto
{
    public long DocId { get; set; }
    public string OrgId { get; set; } = null!;
    public int LocationId { get; set; }
    public string? DocumentNo { get; set; }
    public string DocumentType { get; set; } = null!;
    public long MainCategory { get; set; }
    public long SubCategory { get; set; }
    public string PoNumber { get; set; } = null!;
    public long VendorSiteId { get; set; }
    public long VendorId { get; set; }
    public int DueDays { get; set; }
    public string InvoiceNo { get; set; } = null!;
    public long InvoiceAmount { get; set; }
    public int Currency { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime InvoiceReceiptDate { get; set; }
    public long Pages { get; set; }
    public string? Remarks { get; set; }
    public DateTime PaymentDueDate { get; set; }
    public string DocumentStatus { get; set; } = null!;
    public string? InvoiceStatus { get; set; }
    public long Owner { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? FilePath { get; set; }
    public string? HoldStatus { get; set; }
    public string CancelFlag { get; set; } = "N";
    public long? ApprovedBy { get; set; }
    public List<OracleInvoiceDetailDto> OracleInvoices { get; set; } = [];
    public List<OraclePaymentDetailDto> OraclePayments { get; set; } = [];
    public List<DocumentPoListDto> PoList { get; set; } = [];
    public List<DocumentCostCenterDto> CostCenters { get; set; } = [];
}

public class OracleInvoiceDetailDto
{
    public long InvId { get; set; }
    public long DocId { get; set; }
    public decimal? VoucherNo { get; set; }
    public string? InvoiceType { get; set; }
    public string? InvoiceNum { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public decimal? InvoiceAmount { get; set; }
    public string? InvoiceStatus { get; set; }
    public string? PaymentMethodCode { get; set; }
}

public class OraclePaymentDetailDto
{
    public long PayId { get; set; }
    public long DocId { get; set; }
    public long PaymentNum { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal? GrossAmount { get; set; }
    public decimal? AmountRemaining { get; set; }
    public string? PaymentStatus { get; set; }
    public string? PaymentMethod { get; set; }
    public long? CheckNumber { get; set; }
    public DateTime? CheckDate { get; set; }
    public decimal? CheckAmount { get; set; }
}

public class DocumentPoListDto
{
    public long SeqId { get; set; }
    public long DocId { get; set; }
    public long PoId { get; set; }
    public string PoNo { get; set; } = null!;
    public string PoLineNo { get; set; } = null!;
    public DateTime? PoDate { get; set; }
}

public class DocumentCostCenterDto
{
    public long CcId { get; set; }
    public long DocId { get; set; }
    public string BusinessUnitId { get; set; } = null!;
    public string LocationCode { get; set; } = null!;
    public string AccountCode { get; set; } = null!;
    public string CostCenterCode { get; set; } = null!;
    public long Percentage { get; set; }
}

public class PagedResultDto<T>
{
    public IReadOnlyList<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
