using MasterService.Domain.Common;

namespace MasterService.Domain.Entities;

/// <summary>Aggregate: COMP_FINYEAR</summary>
public sealed class CompanyFinancialYear : AggregateRoot
{
    public long SerialNumber { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public char CloseFlag { get; private set; }

    public bool IsOpen => CloseFlag == 'N';

    private CompanyFinancialYear() { }

    public static CompanyFinancialYear Create(long serialNumber, DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate) throw new ArgumentException("EndDate must be after StartDate.");

        return new CompanyFinancialYear
        {
            SerialNumber = serialNumber,
            StartDate = startDate,
            EndDate = endDate,
            CloseFlag = 'N'
        };
    }

    public void Close() => CloseFlag = 'Y';
    public void Reopen() => CloseFlag = 'N';
}
