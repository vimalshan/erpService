using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.Forex;

public class ForexMain : AggregateRoot<string>
{
    public string TourPlanId { get; private set; } = string.Empty;
    public string PassportNo { get; private set; } = string.Empty;
    public string PassportName { get; private set; } = string.Empty;
    public string PassportLocation { get; private set; } = string.Empty;
    public DateTime PassportExpiryDate { get; private set; }
    public string? Destination { get; private set; }
    public string? Status { get; private set; }
    public string? RequestedOn { get; private set; }
    public string LastModifiedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOn { get; private set; }
    public DateTime? ReceivedOn { get; private set; }
    public string? ReferenceNo { get; private set; }
    public decimal Charges { get; private set; }
    public decimal ServiceTax { get; private set; }
    public decimal EduCess { get; private set; }
    public decimal HeEduCess { get; private set; }
    public decimal RoundingAmount { get; private set; }
    public string? VendorId { get; private set; }
    public string? Currency { get; private set; }
    public decimal TotalValue { get; private set; }
    public string? RecommendedBy { get; private set; }
    public string RequestType { get; private set; } = string.Empty;
    public string AdditionalRemarks { get; private set; } = string.Empty;
    public string AdvanceRefNo { get; private set; } = string.Empty;

    private readonly List<ForexDetail> _details = new();
    private readonly List<ForexChequeDetail> _chequeDetails = new();

    public IReadOnlyCollection<ForexDetail> Details => _details.AsReadOnly();
    public IReadOnlyCollection<ForexChequeDetail> ChequeDetails => _chequeDetails.AsReadOnly();

    protected ForexMain() { }

    public static ForexMain Create(
        string id, string tourPlanId, string passportNo, string passportName,
        string passportLocation, DateTime passportExpiryDate, string requestType,
        string additionalRemarks, string advanceRefNo, string lastModifiedBy)
        => new()
        {
            Id = id,
            TourPlanId = tourPlanId,
            PassportNo = passportNo,
            PassportName = passportName,
            PassportLocation = passportLocation,
            PassportExpiryDate = passportExpiryDate,
            RequestType = requestType,
            AdditionalRemarks = additionalRemarks,
            AdvanceRefNo = advanceRefNo,
            LastModifiedBy = lastModifiedBy,
            LastModifiedOn = DateTime.UtcNow,
            Status = "P",
            Charges = 0,
            ServiceTax = 0,
            EduCess = 0,
            HeEduCess = 0,
            RoundingAmount = 0,
            TotalValue = 0
        };

    public void AddDetail(ForexDetail detail) => _details.Add(detail);
    public void AddChequeDetail(ForexChequeDetail chequeDetail) => _chequeDetails.Add(chequeDetail);
}
