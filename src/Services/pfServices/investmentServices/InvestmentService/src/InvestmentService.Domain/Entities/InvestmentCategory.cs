namespace InvestmentService.Domain.Entities;

public class InvestmentCategory
{
    public int Code { get; set; }
    public string ShortCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public long Denomination { get; set; }
    public int GroupId { get; set; }

    public ICollection<InvestmentSubCategory> SubCategories { get; set; } = new List<InvestmentSubCategory>();
    public ICollection<Investment> Investments { get; set; } = new List<Investment>();
}
