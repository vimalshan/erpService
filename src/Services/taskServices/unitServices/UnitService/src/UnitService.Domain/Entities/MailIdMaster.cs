using UnitService.Domain.ValueObjects;

namespace UnitService.Domain.Entities;

public class MailIdMaster : BaseEntity
{
    public int MailId { get; private set; }
    public UnitCode UnitCode { get; private set; } = null!;
    public string EmailAddress { get; private set; } = string.Empty;
    public string DeliveryType { get; private set; } = string.Empty;
    public string StartDate { get; private set; } = string.Empty;
    public string? CloseDate { get; private set; }
    public int LastModifiedBy { get; private set; }
    public string LastModifiedOn { get; private set; } = string.Empty;
    public string Module { get; private set; } = string.Empty;

    private MailIdMaster() { }

    public static MailIdMaster Create(int mailId, string unitCode, string emailAddress,
        string deliveryType, string module, int modifiedBy)
    {
        return new MailIdMaster
        {
            MailId = mailId,
            UnitCode = UnitCode.From(unitCode),
            EmailAddress = emailAddress,
            DeliveryType = deliveryType,
            StartDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            Module = module,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }
}
