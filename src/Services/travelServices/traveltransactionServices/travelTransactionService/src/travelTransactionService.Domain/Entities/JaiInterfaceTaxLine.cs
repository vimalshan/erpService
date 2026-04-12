using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Entities;

public class JaiInterfaceTaxLine : BaseEntity
{
    public decimal? InterfaceTaxLineId { get; private set; }
    public decimal? InterfaceLineId { get; private set; }
    public decimal PartyId { get; private set; }
    public decimal PartySiteId { get; private set; }
    public string ImportModule { get; private set; } = null!;
    public string TransactionNum { get; private set; } = null!;
    public decimal TransactionLineNum { get; private set; }
    public long TaxLineNo { get; private set; }
    public string? ExternalTaxCode { get; private set; }
    public long? TaxId { get; private set; }
    public decimal? TaxRate { get; private set; }
    public decimal? TaxAmount { get; private set; }
    public decimal? FuncTaxAmount { get; private set; }
    public decimal? BaseTaxAmount { get; private set; }
    public string? InclusiveTaxFlag { get; private set; }
    public long? CodeCombinationId { get; private set; }
    public DateTime CreationDate { get; private set; }
    public decimal CreatedBy { get; private set; }
    public DateTime LastUpdateDate { get; private set; }
    public decimal LastUpdatedBy { get; private set; }
    public long? JvNumber { get; private set; }

    private JaiInterfaceTaxLine() { }

    public static JaiInterfaceTaxLine Create(
        decimal partyId,
        decimal partySiteId,
        string importModule,
        string transactionNum,
        decimal transactionLineNum,
        long taxLineNo,
        decimal? taxRate,
        decimal? taxAmount,
        decimal createdBy)
    {
        return new JaiInterfaceTaxLine
        {
            PartyId = partyId,
            PartySiteId = partySiteId,
            ImportModule = importModule,
            TransactionNum = transactionNum,
            TransactionLineNum = transactionLineNum,
            TaxLineNo = taxLineNo,
            TaxRate = taxRate,
            TaxAmount = taxAmount,
            CreatedBy = createdBy,
            CreationDate = DateTime.UtcNow,
            LastUpdateDate = DateTime.UtcNow,
            LastUpdatedBy = createdBy
        };
    }
}
