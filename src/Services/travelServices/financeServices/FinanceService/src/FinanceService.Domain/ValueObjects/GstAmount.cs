using FinanceService.Domain.Common;

namespace FinanceService.Domain.ValueObjects;

public class GstAmount : ValueObject
{
    public decimal Sgst { get; private set; }
    public decimal Cgst { get; private set; }
    public decimal Igst { get; private set; }
    public decimal Total => Sgst + Cgst + Igst;

    private GstAmount() { }

    public GstAmount(decimal sgst, decimal cgst, decimal igst)
    {
        Sgst = sgst;
        Cgst = cgst;
        Igst = igst;
    }

    public static GstAmount Zero() => new(0, 0, 0);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Sgst;
        yield return Cgst;
        yield return Igst;
    }
}
