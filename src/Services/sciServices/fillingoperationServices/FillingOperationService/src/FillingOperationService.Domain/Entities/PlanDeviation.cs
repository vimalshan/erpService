using FillingOperationService.Domain.Common;

namespace FillingOperationService.Domain.Entities;

public class PlanDeviation : Entity
{
    public int ReasonId { get; private set; }
    public DateTime PlanDate { get; private set; }
    public int FillingLineId { get; private set; }
    public int ProductId { get; private set; }
    public string? Reason { get; private set; }

    protected PlanDeviation() { }

    public static PlanDeviation Create(DateTime planDate, int lineId, int productId, string? reason)
    {
        return new PlanDeviation
        {
            PlanDate = planDate,
            FillingLineId = lineId,
            ProductId = productId,
            Reason = reason
        };
    }
}
