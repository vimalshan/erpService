namespace IntegrationService.Application.DTOs;

public record PurchaseOrderDto
{
    public long PoSeqId { get; init; }
    public long OracleOrgId { get; init; }
    public long OraclePoId { get; init; }
    public string PoNumber { get; init; } = string.Empty;
    public long VendorSiteId { get; init; }
    public long DueDays { get; init; }
    public long DueDayMonthOffset { get; init; }
    public long MonthForward { get; init; }
    public List<MaterialReceiptDto> MaterialReceipts { get; init; } = [];
}

public record MaterialReceiptDto
{
    public long MrcSeqId { get; init; }
    public long PurchaseOrderId { get; init; }
    public string MrcNumber { get; init; } = string.Empty;
    public long? SequenceNumber { get; init; }
    public DateTime? ReceiveDate { get; init; }
    public long? VendorId { get; init; }
    public long? VendorSiteId { get; init; }
}

public record VendorDto
{
    public int VendorId { get; init; }
    public string VendorName { get; init; } = string.Empty;
    public string VendorCode { get; init; } = string.Empty;
    public List<VendorSiteDto> VendorSites { get; init; } = [];
}

public record VendorSiteDto
{
    public long VendorSiteId { get; init; }
    public long VendorId { get; init; }
    public string SiteCode { get; init; } = string.Empty;
    public string OracleOuId { get; init; } = string.Empty;
    public List<VendorSiteBuMappingDto> BuMappings { get; init; } = [];
}

public record VendorSiteBuMappingDto
{
    public long VendorSiteId { get; init; }
    public long BuId { get; init; }
}

public record OrganizationUnitDto
{
    public string OuId { get; init; } = string.Empty;
    public string OuName { get; init; } = string.Empty;
    public string BuId { get; init; } = string.Empty;
    public List<OuBuMappingDto> OuBuMappings { get; init; } = [];
}

public record OuBuMappingDto
{
    public long OuId { get; init; }
    public string BuId { get; init; } = string.Empty;
}
