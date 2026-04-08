namespace SciTransactional.Application.DTOs;

public sealed record NavigationDto
{
    public long RequestNum { get; init; }
    public string UserId { get; init; } = string.Empty;
    public long UserNum { get; init; }
    public string? RandomNum { get; init; }
    public DateTime UpdatedDate { get; init; }
    public string SciId { get; init; } = string.Empty;
    public string? StatusFlag { get; init; }
}

public sealed record NormsMainDto
{
    public long NormNo { get; init; }
    public DateTime EffectiveDate { get; init; }
    public DateTime? ClosureDate { get; init; }
    public IReadOnlyList<NormsMasterDto> Details { get; init; } = [];
}

public sealed record NormsMasterDto
{
    public long NormId { get; init; }
    public int? InputCode { get; init; }
    public int? OutputCode { get; init; }
    public int? Rate { get; init; }
    public long? NormNo { get; init; }
}

public sealed record AdvanceLicenseDto
{
    public long LicenseId { get; init; }
    public string? LicenseNo { get; init; }
    public int? FgCode { get; init; }
    public decimal? ExportObligationAmount { get; init; }
    public decimal? ExportAmount { get; init; }
    public IReadOnlyList<EntitlementDto> Entitlements { get; init; } = [];
}

public sealed record EntitlementDto
{
    public long LicenseId { get; init; }
    public int EntitlementRm { get; init; }
}

public sealed record AutoMailStatusDto
{
    public int Id { get; init; }
    public string MailType { get; init; } = string.Empty;
    public DateTime MailDate { get; init; }
    public string MailStatus { get; init; } = string.Empty;
    public string? MailRemarks { get; init; }
}

public sealed record AutoMailIdDto
{
    public int Id { get; init; }
    public string? IdType { get; init; }
    public string? MailId { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? MailType { get; init; }
}

public sealed record OrderMapDto
{
    public int Id { get; init; }
    public decimal? TiedOrderDetailId { get; init; }
    public decimal? ActualLineId { get; init; }
    public int? MappingQuantity { get; init; }
    public int? ModifiedByUserId { get; init; }
    public DateTime? ModifiedDate { get; init; }
}

public sealed record DirectEntryDto
{
    public long Id { get; init; }
    public long? TrackingNumber { get; init; }
    public DateTime? EnteredDate { get; init; }
    public string? EnteredUser { get; init; }
}
