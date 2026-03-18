using ReviewService.Domain.Common;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to TRAINER_FEED table - trainer feedback.
/// </summary>
public class TrainerFeed : BaseEntity
{
    public long TrGrpCod { get; private set; }
    public long TrFedNum { get; private set; }
    public long? TrSrlNum { get; private set; }
    public string? TrQtnGrp { get; private set; }
    public long? TrGrpNum { get; private set; }
    public long? TrWgtNum { get; private set; }
    public DateTime? TrEffDat { get; private set; }
    public DateTime? TrClsDat { get; private set; }

    private TrainerFeed() { }

    public static TrainerFeed Create(
        long groupCode, long fedNum, long? srlNum,
        string? questionGroup, long? groupNum, long? weightNum,
        DateTime? effectiveDate, DateTime? closingDate)
    {
        return new TrainerFeed
        {
            TrGrpCod = groupCode,
            TrFedNum = fedNum,
            TrSrlNum = srlNum,
            TrQtnGrp = questionGroup,
            TrGrpNum = groupNum,
            TrWgtNum = weightNum,
            TrEffDat = effectiveDate,
            TrClsDat = closingDate
        };
    }
}
