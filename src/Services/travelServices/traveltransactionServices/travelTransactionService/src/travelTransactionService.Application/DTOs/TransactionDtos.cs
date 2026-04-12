namespace travelTransactionService.Application.DTOs;

public record VendorMasterDto
{
    public long VendorId { get; init; }
    public string Name { get; init; } = null!;
    public string? AddressLine1 { get; init; }
    public string? AddressLine2 { get; init; }
    public string? AddressLine3 { get; init; }
    public long? CityCode { get; init; }
    public string? ItPanNumber { get; init; }
    public string? PhoneNumber { get; init; }
    public string? AccountNumber { get; init; }
    public string? BankName { get; init; }
    public string CategoryType { get; init; } = null!;
}

public record AccountMasterDto
{
    public string? CompanyCode { get; init; }
    public string? EdCode { get; init; }
    public string? AccountCode { get; init; }
    public string? GradeType { get; init; }
    public string? DebitCreditFlag { get; init; }
    public string? SubCode { get; init; }
    public string? AccountDescription { get; init; }
}

public record GlCodeCombinationDto
{
    public long RowId { get; init; }
    public long CodeCombinationId { get; init; }
    public long ChartOfAccountsId { get; init; }
    public string? ConcatenatedSegments { get; init; }
    public string GlAccountType { get; init; } = null!;
    public string EnabledFlag { get; init; } = null!;
    public string? Segment1 { get; init; }
    public string? Segment2 { get; init; }
    public string? Segment3 { get; init; }
    public string? Description { get; init; }
}

public record TaxMasterDto
{
    public long TaxVendorId { get; init; }
    public string TaxType { get; init; } = null!;
    public decimal? TaxRate { get; init; }
    public DateTime TaxEffectiveDate { get; init; }
    public DateTime? TaxCloseDate { get; init; }
    public List<TaxComponentDto> Components { get; init; } = [];
}

public record TaxComponentDto
{
    public long VendorCode { get; init; }
    public string? Component { get; init; }
}

public record JvInterfaceDto
{
    public decimal? CodeCombination { get; init; }
    public string? Segment1 { get; init; }
    public decimal? Io { get; init; }
    public string? Unit { get; init; }
}

public record JvMissingCombiCodeDto
{
    public string? AgencyName { get; init; }
    public string? InvoiceNumber { get; init; }
    public string? Description { get; init; }
    public string? DistCodeConcatenated { get; init; }
    public long? JvNumber { get; init; }
    public long? LogSysId { get; init; }
}

public record JaiInterfaceLineDto
{
    public decimal? InterfaceLineId { get; init; }
    public decimal OrgId { get; init; }
    public decimal PartyId { get; init; }
    public decimal PartySiteId { get; init; }
    public string ImportModule { get; init; } = null!;
    public string TransactionNum { get; init; } = null!;
    public decimal TransactionLineNum { get; init; }
    public string? ErrorFlag { get; init; }
    public string? ImportStatus { get; init; }
    public decimal? BatchId { get; init; }
    public decimal? InvoiceId { get; init; }
    public string? Type { get; init; }
    public decimal? SgstAmount { get; init; }
    public decimal? CgstAmount { get; init; }
    public decimal? IgstAmount { get; init; }
    public long? JvNumber { get; init; }
    public List<JaiInterfaceTaxLineDto> TaxLines { get; init; } = [];
}

public record JaiInterfaceTaxLineDto
{
    public decimal? InterfaceTaxLineId { get; init; }
    public decimal? InterfaceLineId { get; init; }
    public long TaxLineNo { get; init; }
    public string? ExternalTaxCode { get; init; }
    public decimal? TaxRate { get; init; }
    public decimal? TaxAmount { get; init; }
    public long? CodeCombinationId { get; init; }
}

public record BatchSubBreakupDto
{
    public long SlNo { get; init; }
    public decimal BookingNumber { get; init; }
    public string CostUnit { get; init; } = null!;
    public string CostCode { get; init; } = null!;
    public string? ProductCode { get; init; }
    public string? SubAccountCode { get; init; }
}

public record TravelApParamsDto
{
    public long ApUnitId { get; init; }
    public string AccountStatus { get; init; } = null!;
    public string AccountCode { get; init; } = null!;
    public long? ControlCombId { get; init; }
}

public record SourceHistoryDto
{
    public DateTime? ChangeDate { get; init; }
    public string? Name { get; init; }
    public string? Type { get; init; }
    public decimal? Line { get; init; }
    public string? Text { get; init; }
}
