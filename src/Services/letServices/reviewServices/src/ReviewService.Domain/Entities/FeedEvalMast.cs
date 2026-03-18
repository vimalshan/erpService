using ReviewService.Domain.Common;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to FEED_EVALMAST table - evaluation master.
/// </summary>
public class FeedEvalMast : BaseEntity
{
    public long? FdEvlTyp { get; private set; }
    public string? FdEvlDes { get; private set; }
    public decimal? FdWgtNum { get; private set; }

    private FeedEvalMast() { }

    public static FeedEvalMast Create(long evalType, string description, decimal weight)
    {
        return new FeedEvalMast
        {
            FdEvlTyp = evalType,
            FdEvlDes = description,
            FdWgtNum = weight
        };
    }
}
