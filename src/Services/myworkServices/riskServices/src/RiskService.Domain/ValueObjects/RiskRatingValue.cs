using RiskService.Domain.Common;

namespace RiskService.Domain.ValueObjects;

public class RiskRatingValue : ValueObject
{
    public long ImpactId { get; private set; }
    public long ProbabilityId { get; private set; }
    public long RatingId { get; private set; }

    private RiskRatingValue() { }

    public RiskRatingValue(long impactId, long probabilityId, long ratingId)
    {
        ImpactId = impactId;
        ProbabilityId = probabilityId;
        RatingId = ratingId;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ImpactId;
        yield return ProbabilityId;
        yield return RatingId;
    }
}
