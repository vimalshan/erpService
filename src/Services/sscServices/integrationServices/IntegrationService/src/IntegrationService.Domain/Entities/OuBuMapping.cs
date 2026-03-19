namespace IntegrationService.Domain.Entities;

public class OuBuMapping
{
    public long OuId { get; private set; }
    public string BuId { get; private set; } = string.Empty;

    private OuBuMapping() { }

    public static OuBuMapping Create(long ouId, string buId)
    {
        return new OuBuMapping
        {
            OuId = ouId,
            BuId = buId
        };
    }
}
