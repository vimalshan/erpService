using ReviewService.Domain.Common;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to COURSE_FEEDSUB table.
/// </summary>
public class CourseFeedSub : BaseEntity
{
    public long FdReqNum { get; private set; }
    public long FdReqSrl { get; private set; }
    public long FdSrlNum { get; private set; }
    public long FdTypCod { get; private set; }
    public long? FdTypNum { get; private set; }
    public string? FdTypDes { get; private set; }

    public CourseFeedMain? FeedMain { get; private set; }

    private CourseFeedSub() { }

    public static CourseFeedSub Create(
        long reqNum, long reqSrl, long srlNum,
        long typeCode, long? typeNum, string? typeDesc)
    {
        return new CourseFeedSub
        {
            FdReqNum = reqNum,
            FdReqSrl = reqSrl,
            FdSrlNum = srlNum,
            FdTypCod = typeCode,
            FdTypNum = typeNum,
            FdTypDes = typeDesc
        };
    }
}
