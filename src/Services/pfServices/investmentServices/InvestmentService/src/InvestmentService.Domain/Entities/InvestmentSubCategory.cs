namespace InvestmentService.Domain.Entities;

public class InvestmentSubCategory
{
    public int Id { get; set; }
    public string ShortName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
    public long? InterestDenomination { get; set; }
    public long? SubCategory { get; set; }

    public InvestmentCategory Category { get; set; } = null!;
    public ICollection<Investment> Investments { get; set; } = new List<Investment>();
}
