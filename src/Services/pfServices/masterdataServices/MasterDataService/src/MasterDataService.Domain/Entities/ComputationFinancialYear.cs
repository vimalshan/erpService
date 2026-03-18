using MasterDataService.Domain.Common;
using MasterDataService.Domain.Events;

namespace MasterDataService.Domain.Entities;

public class ComputationFinancialYear : AggregateRoot
{
    public long SerialNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string CloseFlag { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public string? InterestFlag { get; set; }
    public string? EmployeeName { get; set; }
    public string? EmployeeDesignation { get; set; }
    public long? BatchNumber { get; set; }

    public void CloseYear()
    {
        CloseFlag = "Y";
        AddDomainEvent(new FinancialYearClosedEvent(SerialNumber));
    }
}
