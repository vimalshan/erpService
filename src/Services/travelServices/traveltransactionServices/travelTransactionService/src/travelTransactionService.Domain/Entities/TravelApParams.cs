using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Entities;

public class TravelApParams : BaseEntity
{
    public long ApUnitId { get; private set; }
    public string AccountStatus { get; private set; } = null!;
    public string AccountCode { get; private set; } = null!;
    public long? ControlCombId { get; private set; }

    private TravelApParams() { }

    public static TravelApParams Create(
        long apUnitId,
        string accountStatus,
        string accountCode,
        long? controlCombId = null)
    {
        return new TravelApParams
        {
            ApUnitId = apUnitId,
            AccountStatus = accountStatus,
            AccountCode = accountCode,
            ControlCombId = controlCombId
        };
    }

    public void UpdateAccountCode(string accountCode, long? controlCombId)
    {
        AccountCode = accountCode;
        ControlCombId = controlCombId;
    }
}
