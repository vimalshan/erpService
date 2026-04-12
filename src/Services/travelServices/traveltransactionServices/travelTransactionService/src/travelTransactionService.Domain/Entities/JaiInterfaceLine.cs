using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Entities;

public class JaiInterfaceLine : AggregateRoot
{
    public decimal? InterfaceLineId { get; private set; }
    public decimal OrgId { get; private set; }
    public decimal? OrganizationId { get; private set; }
    public decimal? LocationId { get; private set; }
    public decimal PartyId { get; private set; }
    public decimal PartySiteId { get; private set; }
    public string ImportModule { get; private set; } = null!;
    public decimal? TransactionId { get; private set; }
    public string TransactionNum { get; private set; } = null!;
    public decimal TransactionLineNum { get; private set; }
    public string? ErrorFlag { get; private set; }
    public string? BatchSourceName { get; private set; }
    public string? TaxableBasis { get; private set; }
    public string? TaxableEvent { get; private set; }
    public string? InclusiveTaxAmount { get; private set; }
    public string? ExclusiveTaxAmount { get; private set; }
    public DateTime CreationDate { get; private set; }
    public decimal CreatedBy { get; private set; }
    public DateTime LastUpdateDate { get; private set; }
    public decimal LastUpdatedBy { get; private set; }
    public string? ImportStatus { get; private set; }
    public string? HsnCode { get; private set; }
    public string? SacCode { get; private set; }
    public decimal? BatchId { get; private set; }
    public decimal? InvoiceId { get; private set; }
    public decimal? LineNumber { get; private set; }
    public string? BatchBu { get; private set; }
    public string? Type { get; private set; }
    public string? TypeTour { get; private set; }
    public int? TravelClass { get; private set; }
    public decimal? SgstAmount { get; private set; }
    public decimal? CgstAmount { get; private set; }
    public decimal? IgstAmount { get; private set; }
    public long? JvNumber { get; private set; }
    public long? AgencyId { get; private set; }
    public long? CombinationId { get; private set; }

    private readonly List<JaiInterfaceTaxLine> _taxLines = [];
    public IReadOnlyCollection<JaiInterfaceTaxLine> TaxLines => _taxLines.AsReadOnly();

    private JaiInterfaceLine() { }

    public static JaiInterfaceLine Create(
        decimal orgId,
        decimal partyId,
        decimal partySiteId,
        string importModule,
        string transactionNum,
        decimal transactionLineNum,
        decimal createdBy)
    {
        var line = new JaiInterfaceLine
        {
            OrgId = orgId,
            PartyId = partyId,
            PartySiteId = partySiteId,
            ImportModule = importModule,
            TransactionNum = transactionNum,
            TransactionLineNum = transactionLineNum,
            CreatedBy = createdBy,
            CreationDate = DateTime.UtcNow,
            LastUpdateDate = DateTime.UtcNow,
            LastUpdatedBy = createdBy
        };

        line.AddDomainEvent(new Events.JaiInterfaceLineCreatedEvent(transactionNum, transactionLineNum));
        return line;
    }

    public void AddTaxLine(JaiInterfaceTaxLine taxLine) => _taxLines.Add(taxLine);

    public void UpdateGstAmounts(decimal sgst, decimal cgst, decimal igst)
    {
        SgstAmount = sgst;
        CgstAmount = cgst;
        IgstAmount = igst;
        LastUpdateDate = DateTime.UtcNow;
    }
}
