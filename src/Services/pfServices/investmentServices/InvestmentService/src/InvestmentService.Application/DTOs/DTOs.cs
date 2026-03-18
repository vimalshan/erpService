namespace InvestmentService.Application.DTOs;

public class InvestmentDto
{
    public long InvNo { get; set; }
    public int? GroupId { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int? SubCategoryId { get; set; }
    public string? SubCategoryName { get; set; }
    public int? Tenure { get; set; }
    public int? TenureDays { get; set; }
    public string? InterestOption { get; set; }
    public DateTime? OriginalPurchaseDate { get; set; }
    public DateTime? MaturityDate { get; set; }
    public string? CallPutOption { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal? Units { get; set; }
    public decimal? PurchaseRate { get; set; }
    public decimal? FaceValue { get; set; }
    public decimal? Premium { get; set; }
    public decimal? IssuedInterestRate { get; set; }
    public decimal? PurchaseValue { get; set; }
    public string? Status { get; set; }
    public string? CertificateNumber { get; set; }
    public decimal? YtmRate { get; set; }
    public decimal? NetValue { get; set; }
    public string? BrokerName { get; set; }
    public List<SaleDetailDto> SaleDetails { get; set; } = new();
    public List<ScheduleDetailDto> ScheduleDetails { get; set; } = new();
}

public class SaleDetailDto
{
    public long SaleNo { get; set; }
    public long InvNo { get; set; }
    public string SaleType { get; set; } = null!;
    public DateTime SaleDate { get; set; }
    public decimal InterestAdjusted { get; set; }
    public decimal SalePremium { get; set; }
    public decimal SaleValue { get; set; }
    public string? Remarks { get; set; }
}

public class ScheduleDetailDto
{
    public long SchId { get; set; }
    public long InvNo { get; set; }
    public string ScheduleType { get; set; } = null!;
    public DateTime InterestFrom { get; set; }
    public DateTime InterestTo { get; set; }
    public decimal DueAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal? ReceivedAmount { get; set; }
    public DateTime? ReceivedDate { get; set; }
}

public class CategoryDto
{
    public int Code { get; set; }
    public string ShortCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public long Denomination { get; set; }
    public int GroupId { get; set; }
    public List<SubCategoryDto> SubCategories { get; set; } = new();
}

public class SubCategoryDto
{
    public int Id { get; set; }
    public string ShortName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
}

public class BrokerDto
{
    public decimal BrokerId { get; set; }
    public string BrokerName { get; set; } = null!;
    public string BrokerStatus { get; set; } = null!;
}

public class CreditAgencyDto
{
    public int AgencyId { get; set; }
    public string AgencyName { get; set; } = null!;
}

public class CreditRatingDto
{
    public int RatingId { get; set; }
    public string RatingName { get; set; } = null!;
}

public class PortfolioSummaryDto
{
    public int TotalInvestments { get; set; }
    public int ActiveInvestments { get; set; }
    public int MaturedInvestments { get; set; }
    public int RedeemedInvestments { get; set; }
    public decimal TotalPurchaseValue { get; set; }
    public decimal TotalActiveValue { get; set; }
    public List<CategorySummaryDto> CategorySummaries { get; set; } = new();
}

public class CategorySummaryDto
{
    public string CategoryName { get; set; } = null!;
    public int Count { get; set; }
    public decimal TotalValue { get; set; }
}
