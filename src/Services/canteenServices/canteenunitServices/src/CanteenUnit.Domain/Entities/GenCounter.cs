using CanteenUnit.Domain.Common;

namespace CanteenUnit.Domain.Entities;

/// <summary>Maps to GEN_COUNTER</summary>
public class GenCounter : BaseEntity
{
    public string GnTrnTyp { get; private set; } = null!;   // GN_TRN_TYP PK
    public long? GnTrnNum { get; private set; }              // GN_TRN_NUM
    public string? GnTrnDes { get; private set; }            // GN_TRN_DES

    private GenCounter() { }

    public static GenCounter Create(string transType, long? transNum, string? description)
    {
        if (string.IsNullOrWhiteSpace(transType))
            throw new ArgumentException("Transaction type cannot be empty.", nameof(transType));
        return new GenCounter { GnTrnTyp = transType, GnTrnNum = transNum, GnTrnDes = description };
    }

    public long Increment()
    {
        GnTrnNum = (GnTrnNum ?? 0) + 1;
        return GnTrnNum.Value;
    }
}
