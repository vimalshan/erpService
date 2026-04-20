namespace OverviewService.Models;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public T? Data { get; set; }
}

public class CertificationQuicklinkCardData
{
    public int CurrentPage { get; set; }
    public List<CertificationServiceData> Data { get; set; } = new();
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}

public class CertificationServiceData
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public List<YearData> YearData { get; set; } = new();
}

public class YearData
{
    public int Year { get; set; }
    public List<StatusValue> Values { get; set; } = new();
}

public class StatusValue
{
    public int Count { get; set; }
    public int Seq { get; set; }
    public string Status { get; set; } = string.Empty;
    public int TotalCount { get; set; }
}

public class FinancialStatusItem
{
    public string FinancialStatus { get; set; } = string.Empty;
    public int FinancialCount { get; set; }
    public double FinancialPercentage { get; set; }
}

public class UpcomingAuditData
{
    public int Confirmed { get; set; }
    public int ToBeConfirmed { get; set; }
    public int ToBeConfirmedBySuaadhya { get; set; }
}

public class TrainingStatusData
{
    public int Completed { get; set; }
    public int Pending { get; set; }
    public int InProgress { get; set; }
}

public class QuickLinkCardRequestInput
{
    public int? Company { get; set; }
    public int? Service { get; set; }
    public int? Site { get; set; }
}
