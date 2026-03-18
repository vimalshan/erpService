namespace DispatchPlanning.Domain.ValueObjects;

public sealed record TargetWeeks
{
    public long? Week1 { get; }
    public long? Week2 { get; }
    public long? Week3 { get; }
    public long? Week4 { get; }
    public long? Week5 { get; }
    public long? MPlus1 { get; }
    public long? MPlus2 { get; }
    public long? MPlus3 { get; }
    public long? MPlus4 { get; }

    public TargetWeeks(long? week1, long? week2, long? week3, long? week4, long? week5,
        long? mPlus1, long? mPlus2, long? mPlus3, long? mPlus4)
    {
        Week1 = week1;
        Week2 = week2;
        Week3 = week3;
        Week4 = week4;
        Week5 = week5;
        MPlus1 = mPlus1;
        MPlus2 = mPlus2;
        MPlus3 = mPlus3;
        MPlus4 = mPlus4;
    }

    public long TotalAllWeeks() =>
        (Week1 ?? 0) + (Week2 ?? 0) + (Week3 ?? 0) + (Week4 ?? 0) + (Week5 ?? 0);
}
