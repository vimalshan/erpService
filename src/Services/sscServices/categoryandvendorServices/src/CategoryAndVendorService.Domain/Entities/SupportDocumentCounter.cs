using CategoryAndVendorService.Domain.Common;

namespace CategoryAndVendorService.Domain.Entities;

/// <summary>
/// Entity: Support Document Counter (SUPDOC_COUNTER)
/// </summary>
public class SupportDocumentCounter : Entity
{
    public string BuId { get; private set; } = null!;
    public long CounterNo { get; private set; }

    private SupportDocumentCounter() { }

    public static SupportDocumentCounter Create(string buId, long counterNo = 0)
    {
        return new SupportDocumentCounter
        {
            BuId = buId,
            CounterNo = counterNo
        };
    }

    public long GetNextNumber()
    {
        CounterNo++;
        return CounterNo;
    }
}
