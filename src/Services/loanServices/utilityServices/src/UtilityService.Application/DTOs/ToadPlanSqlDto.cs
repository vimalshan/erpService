namespace UtilityService.Application.DTOs;

public class ToadPlanSqlDto
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string StatementId { get; set; } = string.Empty;
    public DateTime? Timestamp { get; set; }
    public string? Statement { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateToadPlanSqlDto
{
    public string? Username { get; set; }
    public string StatementId { get; set; } = string.Empty;
    public string? Statement { get; set; }
    public DateTime? Timestamp { get; set; }
}

public class UpdateToadPlanSqlDto
{
    public string? Username { get; set; }
    public string? Statement { get; set; }
    public DateTime? Timestamp { get; set; }
}

public class PagedResultDto<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
