namespace TransactionService.Application.DTOs;

// ── Employee JV ──────────────────────────────────────────────────────────────

public sealed record EmployeeJVDto
{
    public long JvBatchId { get; init; }
    public long JvTpId { get; init; }
    public string JvType { get; init; } = default!;
    public DateTime JvDate { get; init; }
    public long JvEmpSysId { get; init; }
    public string JvStatus { get; init; } = default!;
    public string JvTrnType { get; init; } = default!;
    public string? JvOraRefNo { get; init; }
    public decimal JvNetAmt { get; init; }
    public long JvPayUnitId { get; init; }
    public long? JvTrnRefNo { get; init; }
    public IEnumerable<EmployeeJVLineDto> Lines { get; init; } = [];
}

public sealed record EmployeeJVLineDto
{
    public long JvSubId { get; init; }
    public long JvBatchId { get; init; }
    public string JvBu { get; init; } = default!;
    public string JvAcCode { get; init; } = default!;
    public string JvSubAcc { get; init; } = default!;
    public string JvCcCode { get; init; } = default!;
    public string JvProduct { get; init; } = default!;
    public string JvDcFlag { get; init; } = default!;
    public string JvTrnAmt { get; init; } = default!;
    public string JvRemarks { get; init; } = default!;
    public string JvSubType { get; init; } = default!;
}

// ── Supplier JV ──────────────────────────────────────────────────────────────

public sealed record SupplierJVDto
{
    public long JvId { get; init; }
    public string JvType { get; init; } = default!;
    public DateTime JvDate { get; init; }
    public long JvVendorId { get; init; }
    public string? JvOraRefNo { get; init; }
    public string JvStatus { get; init; } = default!;
    public string JvRefInvNo { get; init; } = default!;
    public decimal JvNetAmt { get; init; }
    public string JvTrnType { get; init; } = default!;
    public long JvAdminId { get; init; }
    public IEnumerable<SupplierJVLineDto> Lines { get; init; } = [];
}

public sealed record SupplierJVLineDto
{
    public long JvSubId { get; init; }
    public long JvId { get; init; }
    public string JvBu { get; init; } = default!;
    public string JvAcCode { get; init; } = default!;
    public string JvDcFlag { get; init; } = default!;
    public decimal JvTrnAmt { get; init; }
    public string JvRemarks { get; init; } = default!;
    public string JvSubType { get; init; } = default!;
}

// ── Travel Batch ─────────────────────────────────────────────────────────────

public sealed record TravelBatchDto
{
    public string BatchId { get; init; } = default!;
    public string? AdminId { get; init; }
    public string? PayUnitId { get; init; }
    public DateTime? BatchDate { get; init; }
    public string? InvNum { get; init; }
    public string? InvAmount { get; init; }
    public string? Status { get; init; }
    public string? VendorId { get; init; }
    public string? ApprovedAmount { get; init; }
    public string? TotalPayable { get; init; }
    public string? JvId { get; init; }
    public string? BatchType { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime? CreatedOn { get; init; }
    public IEnumerable<TravelBatchSubDto> SubItems { get; init; } = [];
}

public sealed record TravelBatchSubDto
{
    public string BatchSubId { get; init; } = default!;
    public string BatchId { get; init; } = default!;
    public string? BookCnfId { get; init; }
    public string? BasAmt { get; init; }
    public string? TotAmt { get; init; }
    public string? AppAmt { get; init; }
    public string CreditType { get; init; } = default!;
    public string? TpId { get; init; }
}

// ── Employee Payment ─────────────────────────────────────────────────────────

public sealed record EmployeePaymentDto
{
    public long PayId { get; init; }
    public long PayTpId { get; init; }
    public string PayTrnType { get; init; } = default!;
    public long PayEmpSysId { get; init; }
    public long PayUnitId { get; init; }
    public string PayMode { get; init; } = default!;
    public string PayType { get; init; } = default!;
    public DateTime? PayDate { get; init; }
    public decimal PayAmount { get; init; }
    public long PayRefId { get; init; }
    public long PayBatchId { get; init; }
    public long PayJvId { get; init; }
}

// ── Airline Invoice ──────────────────────────────────────────────────────────

public sealed record AirlineInvoiceDto
{
    public string? AirTicketId { get; init; }
    public string? BookCnfId { get; init; }
    public string? TicketNumber { get; init; }
    public string? PnrNumber { get; init; }
    public string? AirlineVendorId { get; init; }
    public DateTime? EntryDate { get; init; }
    public string? InvoiceNumber { get; init; }
    public DateTime? InvoiceDate { get; init; }
    public string? InvoiceCost { get; init; }
    public string? Cgst { get; init; }
    public string? Sgst { get; init; }
    public string? Igst { get; init; }
    public string? DebitCredit { get; init; }
    public string? VendorAttachment { get; init; }
}
