using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.Forex;

public class ForexChequeDetail : Entity<string>
{
    public string ForexRequestId { get; private set; } = string.Empty;
    public string ChequeNo { get; private set; } = string.Empty;
    public DateTime? ChequeDate { get; private set; }
    public string BankName { get; private set; } = string.Empty;

    protected ForexChequeDetail() { }

    public static ForexChequeDetail Create(
        string id, string forexRequestId, string chequeNo,
        string bankName, DateTime? chequeDate = null)
        => new()
        {
            Id = id,
            ForexRequestId = forexRequestId,
            ChequeNo = chequeNo,
            BankName = bankName,
            ChequeDate = chequeDate
        };
}
