namespace TrustService.Application.DTOs;

public record TrustMasterDto
{
    public string TrustCode { get; init; } = string.Empty;
    public string TrustShortName { get; init; } = string.Empty;
    public string TrustType { get; init; } = string.Empty;
    public DateTime TrustStartDate { get; init; }
    public DateTime? TrustClosureDate { get; init; }
    public string? TrustId { get; init; }
    public string AddressLine1 { get; init; } = string.Empty;
    public string? AddressLine2 { get; init; }
    public string? AddressLine3 { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? PinCode { get; init; }
    public string? Country { get; init; }
    public string? PhoneNo { get; init; }
    public string? FaxNo { get; init; }
    public string? Email { get; init; }
    public string TrustStatus { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public string? RegistrarName { get; init; }
    public string? RegistrarPhone { get; init; }
    public List<TrustFundTypeDto> FundTypes { get; init; } = new();
    public List<TrustRoleDto> Roles { get; init; } = new();
    public List<TrustUnitDto> Units { get; init; } = new();
    public List<TrustApproverDto> Approvers { get; init; } = new();
    public List<TrustConfigurationDto> Configurations { get; init; } = new();
}

public record TrustFundTypeDto
{
    public string FundTrustCode { get; init; } = string.Empty;
    public string FundType { get; init; } = string.Empty;
    public string FundName { get; init; } = string.Empty;
    public string FundPrefix { get; init; } = string.Empty;
    public string FundStatus { get; init; } = string.Empty;
}

public record TrustRoleDto
{
    public string TrTrustCode { get; init; } = string.Empty;
    public int TrRoleId { get; init; }
    public string TrRoleCode { get; init; } = string.Empty;
    public string TrUserId { get; init; } = string.Empty;
    public long TrUserNo { get; init; }
    public DateTime TrEffDate { get; init; }
    public DateTime? TrClsDate { get; init; }
    public string TrStatus { get; init; } = string.Empty;
}

public record TrustApproverDto
{
    public long ApproverId { get; init; }
    public string TrustCode { get; init; } = string.Empty;
    public long ApproverSysId { get; init; }
    public int ApproverLevel { get; init; }
    public string ApproverType { get; init; } = string.Empty;
    public DateTime EffDate { get; init; }
    public DateTime? ClsDate { get; init; }
    public string ApproverStatus { get; init; } = string.Empty;
}

public record TrustConfigurationDto
{
    public long ConfigId { get; init; }
    public string TrustCode { get; init; } = string.Empty;
    public string ConfigName { get; init; } = string.Empty;
    public string ConfigValue { get; init; } = string.Empty;
    public string ConfigCategory { get; init; } = string.Empty;
    public DateTime EffDate { get; init; }
    public DateTime? ClsDate { get; init; }
}

public record TrustUnitDto
{
    public long UnitId { get; init; }
    public string TrustCode { get; init; } = string.Empty;
    public string UnitCode { get; init; } = string.Empty;
    public string UnitName { get; init; } = string.Empty;
    public string UnitType { get; init; } = string.Empty;
    public string AddressLine1 { get; init; } = string.Empty;
    public string? AddressLine2 { get; init; }
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public long? UnitHeadSysId { get; init; }
    public DateTime EffDate { get; init; }
    public DateTime? ClsDate { get; init; }
    public string UnitStatus { get; init; } = string.Empty;
}

public record TrustAuditLogDto
{
    public long AuditId { get; init; }
    public string TrustCode { get; init; } = string.Empty;
    public string AuditAction { get; init; } = string.Empty;
    public string AuditTable { get; init; } = string.Empty;
    public DateTime AuditTimestamp { get; init; }
    public long AuditUserId { get; init; }
    public string? OldValues { get; init; }
    public string? NewValues { get; init; }
}
