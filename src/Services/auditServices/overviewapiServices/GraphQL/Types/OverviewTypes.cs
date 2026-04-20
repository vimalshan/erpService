namespace OverviewService.GraphQL.Types;

public class CertificationQuicklinkCardDataType
{
    public int CurrentPage { get; set; }
    public List<CertificationServiceDataType> Data { get; set; } = new();
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}

public class CertificationServiceDataType
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public List<YearDataType> YearData { get; set; } = new();
}

public class YearDataType
{
    public int Year { get; set; }
    public List<StatusValueType> Values { get; set; } = new();
}

public class StatusValueType
{
    public int Count { get; set; }
    public int Seq { get; set; }
    public string Status { get; set; } = string.Empty;
    public int TotalCount { get; set; }
}

public class FinancialStatusItemType
{
    public string FinancialStatus { get; set; } = string.Empty;
    public int FinancialCount { get; set; }
    public double FinancialPercentage { get; set; }
}

public class UpcomingAuditDataType
{
    public int Confirmed { get; set; }
    public int ToBeConfirmed { get; set; }
    public int ToBeConfirmedBySuaadhya { get; set; }
}

public class TrainingStatusDataType
{
    public int Completed { get; set; }
    public int Pending { get; set; }
    public int InProgress { get; set; }
}
