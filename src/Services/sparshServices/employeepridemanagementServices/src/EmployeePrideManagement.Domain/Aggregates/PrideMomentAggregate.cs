using EmployeePrideManagement.Domain.Entities;

namespace EmployeePrideManagement.Domain.Aggregates;

public class PrideMomentAggregate
{
    public MomentPride PrideMoment { get; }

    public PrideMomentAggregate(MomentPride prideMoment)
    {
        PrideMoment = prideMoment ?? throw new ArgumentNullException(nameof(prideMoment));
    }

    public static PrideMomentAggregate Create(
        string title,
        string? body,
        decimal employeeSysId,
        string footer,
        string location,
        string imagePath,
        long modifiedBy)
    {
        var moment = new MomentPride(title, body, employeeSysId, footer, location, imagePath, modifiedBy);
        return new PrideMomentAggregate(moment);
    }

    public void Update(
        string title,
        string? body,
        string footer,
        string location,
        string imagePath,
        long modifiedBy)
    {
        PrideMoment.Update(title, body, footer, location, imagePath, modifiedBy);
    }
}
