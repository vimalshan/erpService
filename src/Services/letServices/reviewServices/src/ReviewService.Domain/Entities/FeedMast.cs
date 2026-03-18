using ReviewService.Domain.Common;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to FEED_MAST table - feedback type master.
/// </summary>
public class FeedMast : BaseEntity
{
    public long FdTypCod { get; private set; }
    public string FdTypNam { get; private set; } = string.Empty;
    public char FdNumTyp { get; private set; }
    public string? FdEvlCod { get; private set; }

    private FeedMast() { }

    public static FeedMast Create(long typeCode, string typeName, char numType, string? evalCode)
    {
        return new FeedMast
        {
            FdTypCod = typeCode,
            FdTypNam = typeName,
            FdNumTyp = numType,
            FdEvlCod = evalCode
        };
    }
}
