using TrustService.Domain.Common;

namespace TrustService.Domain.Entities;

public class TrustFundType : BaseEntity
{
    public string FundTrustCode { get; private set; } = string.Empty;
    public string FundType { get; private set; } = string.Empty;
    public string FundName { get; private set; } = string.Empty;
    public string FundPrefix { get; private set; } = string.Empty;
    public string FundStatus { get; private set; } = "A";

    public TrustMaster Trust { get; private set; } = null!;

    private TrustFundType() { }

    public static TrustFundType Create(string trustCode, string fundType, string fundName, string fundPrefix)
    {
        return new TrustFundType
        {
            FundTrustCode = trustCode,
            FundType = fundType,
            FundName = fundName,
            FundPrefix = fundPrefix,
            FundStatus = "A"
        };
    }

    public void Deactivate()
    {
        FundStatus = "I";
    }
}
