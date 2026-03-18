namespace CheckupManagementService.Application.Queries;

using MediatR;
using CheckupManagementService.DTOs;

/// <summary>
/// Query to get all checkups with pagination
/// </summary>
public class GetCheckupsQuery : IRequest<GetCheckupsResponse>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Status { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? CheckupType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

/// <summary>
/// Query to get checkup by ID
/// </summary>
public class GetCheckupByIdQuery : IRequest<CheckupMasterDto?>
{
    public string CheckupMasterId { get; set; } = string.Empty;
}

/// <summary>
/// Query to get checkups by employee
/// </summary>
public class GetCheckupsByEmployeeQuery : IRequest<GetCheckupsResponse>
{
    public string EmployeeNumber { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// Query to get health examination by ID
/// </summary>
public class GetHealthExaminationQuery : IRequest<HealthMainDto?>
{
    public string HealthId { get; set; } = string.Empty;
}

/// <summary>
/// Query to get test masters
/// </summary>
public class GetTestMastersQuery : IRequest<GetTestMastersResponse>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool? IsActive { get; set; }
    public string? Category { get; set; }
}

/// <summary>
/// Query to get checkup others details
/// </summary>
public class GetCheckupOthersQuery : IRequest<CheckupOthersDto?>
{
    public string CheckupMasterId { get; set; } = string.Empty;
}

/// <summary>
/// Query to get health check card
/// </summary>
public class GetHealthCheckCardQuery : IRequest<HealthCheckCardDto?>
{
    public string CardNumber { get; set; } = string.Empty;
}

/// <summary>
/// Query to get checkup status report
/// </summary>
public class GetCheckupStatusReportQuery : IRequest<CheckupStatusReportDto>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

// Response types
public class GetCheckupsResponse
{
    public List<CheckupMasterDto> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}

public class GetTestMastersResponse
{
    public List<TestMasterDto> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
}

public class CheckupStatusReportDto
{
    public int TotalCheckups { get; set; }
    public int PendingCheckups { get; set; }
    public int CompletedCheckups { get; set; }
    public int ApprovedCheckups { get; set; }
    public decimal CompletionRate { get; set; }
    public List<CheckupStatusSummary> TopCheckupTypes { get; set; } = new();
    public DateTime ReportDate { get; set; }
}

public class CheckupStatusSummary
{
    public string CheckupType { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal Percentage { get; set; }
}
