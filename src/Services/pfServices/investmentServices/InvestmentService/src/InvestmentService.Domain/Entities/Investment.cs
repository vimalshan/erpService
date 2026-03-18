using InvestmentService.Domain.Common;
using InvestmentService.Domain.Enums;
using InvestmentService.Domain.Events;

namespace InvestmentService.Domain.Entities;

public class Investment : BaseEntity
{
    public long InvNo { get; set; }
    public int? GroupId { get; set; }
    public int? CategoryId { get; set; }
    public int? SubCategoryId { get; set; }
    public int? Tenure { get; set; }
    public int? TenureDays { get; set; }
    public string? InterestOption { get; set; }
    public DateTime? OriginalPurchaseDate { get; set; }
    public DateTime? MaturityDate { get; set; }
    public string? CallPutOption { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal? CallPercentage { get; set; }
    public decimal? Units { get; set; }
    public decimal? PurchaseRate { get; set; }
    public decimal? FaceValue { get; set; }
    public decimal? Premium { get; set; }
    public decimal? IssuedInterestRate { get; set; }
    public DateTime? RevisedInterestFrom { get; set; }
    public decimal? RevisedInterestRate { get; set; }
    public decimal? InterestDenomination { get; set; }
    public decimal? PurchaseValue { get; set; }
    public string? SecondaryMarket { get; set; }
    public decimal? BrokerId { get; set; }
    public DateTime? CumulativeInterestFrom { get; set; }
    public DateTime? CumulativeInterestTo { get; set; }
    public decimal? CumulativeInterestAmount { get; set; }
    public int? CumulativeInterestDays { get; set; }
    public int? CreditAgency1 { get; set; }
    public int? CreditAgency2 { get; set; }
    public int? Rating1 { get; set; }
    public int? Rating2 { get; set; }
    public string? ClientId { get; set; }
    public string? InterestFrequency { get; set; }
    public string? PaymentMode { get; set; }
    public string? InterestDates { get; set; }
    public int? BankId { get; set; }
    public string? ChequeNumber { get; set; }
    public DateTime? ChequeDate { get; set; }
    public decimal? BankCharges { get; set; }
    public string? Status { get; set; }
    public string? CertificateNumber { get; set; }
    public long? EnteredBy { get; set; }
    public DateTime? EnteredOn { get; set; }
    public long? LastModBy { get; set; }
    public DateTime? LastModOn { get; set; }
    public DateTime? LastScheduleDate { get; set; }
    public decimal? YtmRate { get; set; }
    public decimal? NetValue { get; set; }

    // Navigation properties
    public InvestmentCategory? Category { get; set; }
    public InvestmentSubCategory? SubCategory { get; set; }
    public Broker? Broker { get; set; }
    public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    public ICollection<ScheduleDetail> ScheduleDetails { get; set; } = new List<ScheduleDetail>();
    public ICollection<CallDetail> CallDetails { get; set; } = new List<CallDetail>();
    public ICollection<ApprovalDetail> ApprovalDetails { get; set; } = new List<ApprovalDetail>();
    public ICollection<BankDetail> BankDetails { get; set; } = new List<BankDetail>();

    public void RecordPurchase(int categoryId, decimal units, decimal purchaseRate,
        DateTime purchaseDate, DateTime maturityDate, decimal interestRate, long enteredBy)
    {
        CategoryId = categoryId;
        Units = units;
        PurchaseRate = purchaseRate;
        PurchaseDate = purchaseDate;
        MaturityDate = maturityDate;
        IssuedInterestRate = interestRate;
        PurchaseValue = units * purchaseRate;
        Status = "A";
        EnteredBy = enteredBy;
        EnteredOn = DateTime.UtcNow;

        AddDomainEvent(new InvestmentPurchasedEvent(InvNo, purchaseDate, PurchaseValue.Value));
    }

    public void Redeem(long saleNo, string saleType, DateTime saleDate, decimal saleValue, long enteredBy)
    {
        var sale = new SaleDetail
        {
            SaleNo = saleNo,
            InvNo = InvNo,
            SaleType = saleType,
            SaleDate = saleDate,
            InterestAdjusted = 0,
            SalePremium = 0,
            SaleValue = saleValue,
            SaleTransactionId = 0,
            EnteredBy = enteredBy,
            EnteredOn = DateTime.UtcNow,
            LastModBy = enteredBy,
            LastModOn = DateTime.UtcNow
        };

        SaleDetails.Add(sale);
        Status = "R";

        AddDomainEvent(new InvestmentRedeemedEvent(InvNo, saleDate, saleValue));
    }

    public void Mature()
    {
        Status = "M";
        AddDomainEvent(new InvestmentMaturedEvent(InvNo, MaturityDate ?? DateTime.UtcNow));
    }
}
