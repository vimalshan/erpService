using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseService.Infrastructure.Data;

public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options) { }

    public DbSet<TravelExpense> TravelExpenses => Set<TravelExpense>();
    public DbSet<TravelExpenseAllocation> TravelExpenseAllocations => Set<TravelExpenseAllocation>();
    public DbSet<TravelExpenseSub> TravelExpenseSubs => Set<TravelExpenseSub>();
    public DbSet<TravelConveyance> TravelConveyances => Set<TravelConveyance>();
    public DbSet<TravelCurrency> TravelCurrencies => Set<TravelCurrency>();
    public DbSet<DaBreakup> DaBreakups => Set<DaBreakup>();
    public DbSet<DaSummary> DaSummaries => Set<DaSummary>();
    public DbSet<ExpSettlement> ExpSettlements => Set<ExpSettlement>();
    public DbSet<ExpSettlementReport> ExpSettlementReports => Set<ExpSettlementReport>();
    public DbSet<RuleDa> RuleDas => Set<RuleDa>();
    public DbSet<DaRule> DaRules => Set<DaRule>();
    public DbSet<RuleMode> RuleModes => Set<RuleMode>();
    public DbSet<RuleStay> RuleStays => Set<RuleStay>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
