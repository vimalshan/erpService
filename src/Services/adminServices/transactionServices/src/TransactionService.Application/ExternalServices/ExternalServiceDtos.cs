namespace TransactionService.Application.ExternalServices;

// ── Vendor Service (Port 5003) ──
public record VendorDto(
    long Id,
    long CategoryId,
    long LocationId,
    string Name,
    string? Email,
    string Address,
    long UpdatedBy,
    DateTime UpdatedOn,
    string LiveStatus);

// ── Location Service (Port 5002) ──
public record LocationAppMapDto(
    decimal LocationId,
    string AppName,
    long? SiteCategoryCode,
    string? SelfAccess,
    string? DeemedApproval,
    bool IsActive,
    DateTime CreatedDate,
    string? CreatedBy,
    DateTime? ModifiedDate,
    string? ModifiedBy);

public record LocationAppMapSummary(
    decimal LocationId,
    string AppName,
    bool IsActive,
    long? SiteCategoryCode);

// ── Financial Year Service (Port 5001) ──
public record FinancialYearDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int DurationInDays { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}

// ── Stationery Service (Port 5005) ──
public record StationeryItemDto(
    long Id,
    string Description,
    long CatId,
    long LocId,
    long UomId,
    string Make,
    long? PricePerUnit,
    long? ReorderLevel,
    long OpeningStock,
    string Closed);

// ── LOV Service (Port 5007) ──
public record LovTypeDto(long LovTypeId, string LovTypeName);

public record LovMasterDto(
    long LovId,
    long LovTypeId,
    string LovName,
    long LovUpdatedBy,
    DateTime LovUpdatedOn);

public record ItemDataDto(
    int Id,
    string? CatName,
    string? ItemName,
    string? Make,
    string? Uom,
    int? Price);

// ── Common ──
public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize);
