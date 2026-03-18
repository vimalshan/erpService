using LoanManagement.Domain.Common;
using LoanManagement.Domain.Enums;

namespace LoanManagement.Domain.Entities;

public class LoanInterest : BaseEntity
{
    public long IntId { get; private set; }
    public decimal? IntLoanId { get; private set; }
    public string? IntRateType { get; private set; }   // FX or FL
    public decimal? IntPer { get; private set; }
    public long? IntFloatTypeId { get; private set; }
    public DateTime? IntEffDate { get; private set; }
    public DateTime? IntClsDate { get; private set; }

    private LoanInterest() { }

    public static LoanInterest Create(
        long intId,
        decimal loanId,
        InterestRateType rateType,
        decimal percentage,
        long? floatTypeId,
        DateTime effectiveDate)
    {
        if (percentage < 0)
            throw new ArgumentException("Interest percentage cannot be negative.", nameof(percentage));

        return new LoanInterest
        {
            IntId = intId,
            IntLoanId = loanId,
            IntRateType = rateType == InterestRateType.Fixed ? "FX" : "FL",
            IntPer = percentage,
            IntFloatTypeId = rateType == InterestRateType.Floating ? floatTypeId : null,
            IntEffDate = effectiveDate
        };
    }

    public void Close(DateTime closureDate)
    {
        IntClsDate = closureDate;
    }

    public bool IsFixed => IntRateType == "FX";
    public bool IsFloating => IntRateType == "FL";
}
