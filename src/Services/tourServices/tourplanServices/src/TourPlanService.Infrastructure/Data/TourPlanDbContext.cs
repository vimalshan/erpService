using Microsoft.EntityFrameworkCore;
using TourPlanService.Domain.Entities;

namespace TourPlanService.Infrastructure.Data;

public sealed class TourPlanDbContext(DbContextOptions<TourPlanDbContext> options) : DbContext(options)
{
    public DbSet<TourPlan> TourPlans => Set<TourPlan>();
    public DbSet<TourAdvance> TourAdvances => Set<TourAdvance>();
    public DbSet<TourAgenda> TourAgendas => Set<TourAgenda>();
    public DbSet<TourCostCentre> TourCostCentres => Set<TourCostCentre>();
    public DbSet<TourDaBreak> TourDaBreaks => Set<TourDaBreak>();
    public DbSet<TourExpense> TourExpenses => Set<TourExpense>();
    public DbSet<InternationalSchedule> InternationalSchedules => Set<InternationalSchedule>();
    public DbSet<TourLeave> TourLeaves => Set<TourLeave>();
    public DbSet<NmsSchedule> NmsSchedules => Set<NmsSchedule>();
    public DbSet<SelfExpense> SelfExpenses => Set<SelfExpense>();
    public DbSet<ForexRequisition> ForexRequisitions => Set<ForexRequisition>();
    public DbSet<ForexDetail> ForexDetails => Set<ForexDetail>();
    public DbSet<ForexChequeDetail> ForexChequeDetails => Set<ForexChequeDetail>();
    public DbSet<DomesticDaBreak> DomesticDaBreaks => Set<DomesticDaBreak>();
    public DbSet<ForeignExpenseMain> ForeignExpenseMains => Set<ForeignExpenseMain>();
    public DbSet<ForeignExpenseDetail> ForeignExpenseDetails => Set<ForeignExpenseDetail>();
    public DbSet<ForeignExpenseBreakup> ForeignExpenseBreakups => Set<ForeignExpenseBreakup>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TourPlanDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
