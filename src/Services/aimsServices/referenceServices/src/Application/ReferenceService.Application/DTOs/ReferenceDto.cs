namespace ReferenceService.Application.DTOs;

/// <summary>
/// DTO for LOV Type.
/// </summary>
public record LovTypeDto
{
    public int Id { get; init; }
    public string TypeName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int Sequence { get; init; }
    public string Status { get; init; } = "Active";
    public long LastModifiedBy { get; init; }
    public DateTime LastModifiedOn { get; init; }
    public List<LovValueDto> Values { get; init; } = [];
}

/// <summary>
/// DTO for LOV Value.
/// </summary>
public record LovValueDto
{
    public int Id { get; init; }
    public int TypeId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? LongDescription { get; init; }
    public int Sequence { get; init; }
    public string Status { get; init; } = "Active";
    public long LastModifiedBy { get; init; }
    public DateTime LastModifiedOn { get; init; }
}

/// <summary>
/// DTO for Permission Rule.
/// </summary>
public record PermissionRuleDto
{
    public int Id { get; init; }
    public string ResourceId { get; init; } = string.Empty;
    public string Action { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string AppCode { get; init; } = string.Empty;
    public string Status { get; init; } = "Active";
    public long LastModifiedBy { get; init; }
    public DateTime LastModifiedOn { get; init; }
}

/// <summary>
/// DTO for Leave Flag.
/// </summary>
public record LeaveFlagDto
{
    public int Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? Type { get; init; }
    public string Status { get; init; } = "Active";
    public long LastModifiedBy { get; init; }
    public DateTime LastModifiedOn { get; init; }
}

/// <summary>
/// API Response wrapper.
/// </summary>
public record ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int? StatusCode { get; set; }
}

/// <summary>
/// Paginated response.
/// </summary>
public record PaginatedResponse<T>
{
    public List<T> Items { get; init; } = [];
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
