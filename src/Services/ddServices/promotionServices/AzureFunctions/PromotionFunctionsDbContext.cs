using Microsoft.EntityFrameworkCore;

namespace PromotionService.Functions;

/// <summary>
/// Lightweight read-only DbContext used by Azure Functions to query promotion data.
/// Only includes tables needed for scheduled processing; no migrations run here.
/// </summary>
public class PromotionFunctionsDbContext : DbContext
{
    public PromotionFunctionsDbContext(DbContextOptions<PromotionFunctionsDbContext> options) : base(options) { }

    public DbSet<RatingRow> Ratings => Set<RatingRow>();
    public DbSet<PromotionPeriodRow> PromotionPeriods => Set<PromotionPeriodRow>();
    public DbSet<PromotionRecommendationRow> PromotionRecommendations => Set<PromotionRecommendationRow>();
    public DbSet<IncrementRequestRow> IncrementRequests => Set<IncrementRequestRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RatingRow>().ToTable("DD_RATING").HasKey(r => r.RatingId);
        modelBuilder.Entity<PromotionPeriodRow>().ToTable("DD_PROMOTIONPERIOD").HasKey(p => p.PeriodId);
        modelBuilder.Entity<PromotionRecommendationRow>().ToTable("DD_CTGPROMOTION").HasKey(r => r.TransactionId);
        modelBuilder.Entity<IncrementRequestRow>().ToTable("DD_INCDIRECT").HasKey(i => i.IncDirectId);
    }
}

// Lightweight POCOs (read-only projections for function processing)
public class RatingRow
{
    public decimal RatingId { get; set; }
    public decimal EmployeeSystemId { get; set; }
    public int? DDYear { get; set; }
    public string? FinalRating { get; set; }
    public sbyte? IsFinalized { get; set; }
}

public class PromotionPeriodRow
{
    public decimal PeriodId { get; set; }
    public string? PeriodName { get; set; }
    public sbyte IsActive { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class PromotionRecommendationRow
{
    public decimal TransactionId { get; set; }
    public decimal EmployeeSystemId { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public class IncrementRequestRow
{
    public decimal IncDirectId { get; set; }
    public decimal EmployeeSystemId { get; set; }
    public string? Status { get; set; }
    public decimal? Amount { get; set; }
}
