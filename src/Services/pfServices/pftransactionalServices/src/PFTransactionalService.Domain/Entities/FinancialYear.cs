using PFTransactionalService.Domain.Common;
using PFTransactionalService.Domain.Enums;

namespace PFTransactionalService.Domain.Entities;

public class FinancialYear : BaseEntity
{
    public long AcSrlNum { get; private set; }
    public DateTime AcStrDat { get; private set; }
    public DateTime AcEndDat { get; private set; }
    public FinancialYearStatus AcClsFlg { get; private set; }
    public string? AcRemarks { get; private set; }
    public string? AcIntFlg { get; private set; }
    public string? AcEmpName { get; private set; }
    public string? AcEmpDesg { get; private set; }
    public long? AcBatNum { get; private set; }

    private FinancialYear() { }

    public FinancialYear(long srlNum, DateTime startDate, DateTime endDate, string? remarks = null)
    {
        AcSrlNum = srlNum;
        AcStrDat = startDate;
        AcEndDat = endDate;
        AcClsFlg = FinancialYearStatus.Open;
        AcRemarks = remarks;
    }

    public void Close()
    {
        if (AcClsFlg == FinancialYearStatus.Closed)
            throw new InvalidOperationException("Financial year is already closed.");
        AcClsFlg = FinancialYearStatus.Closed;
    }

    public void SetInterestFlag(string flag)
    {
        AcIntFlg = flag;
    }
}
