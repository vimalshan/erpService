using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Entities;

public class JvMissingCombiCode : BaseEntity
{
    public string? AgencyName { get; private set; }
    public string? InvoiceNumber { get; private set; }
    public string? Description { get; private set; }
    public string? DistCodeConcatenated { get; private set; }
    public long? JvNumber { get; private set; }
    public long? LogSysId { get; private set; }

    private JvMissingCombiCode() { }

    public static JvMissingCombiCode Create(
        string? agencyName,
        string? invoiceNumber,
        string? description,
        string? distCodeConcatenated,
        long? jvNumber,
        long? logSysId)
    {
        return new JvMissingCombiCode
        {
            AgencyName = agencyName,
            InvoiceNumber = invoiceNumber,
            Description = description,
            DistCodeConcatenated = distCodeConcatenated,
            JvNumber = jvNumber,
            LogSysId = logSysId
        };
    }
}
