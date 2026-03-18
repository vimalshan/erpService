using AccountingService.Application.Common.Interfaces;
using AccountingService.Domain.Common;
using AccountingService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Infrastructure.Data;

public class AccountingDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;

    public AccountingDbContext(DbContextOptions<AccountingDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<AccountDetail> AccountDetails => Set<AccountDetail>();
    public DbSet<AccountLookup> AccountLookups => Set<AccountLookup>();
    public DbSet<MainAccount> MainAccounts => Set<MainAccount>();
    public DbSet<TransactionDetail> TransactionDetails => Set<TransactionDetail>();
    public DbSet<TransactionMaster> TransactionMasters => Set<TransactionMaster>();
    public DbSet<PfSubAccount> PfSubAccounts => Set<PfSubAccount>();
    public DbSet<GlPosting> GlPostings => Set<GlPosting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountingDbContext).Assembly);

        // ── Seed Data ────────────────────────────────────────────────────────
        SeedMainAccounts(modelBuilder);
        SeedTransactionMasters(modelBuilder);
        SeedPfSubAccounts(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void SeedMainAccounts(ModelBuilder modelBuilder)
    {
        // Using the private constructor via object-based HasData overload
        modelBuilder.Entity<Domain.Entities.MainAccount>().HasData(
            CreateMainAccount("100000", "Cash and Cash Equivalents",   "Cash"),
            CreateMainAccount("110000", "Bank Accounts",               "Bank"),
            CreateMainAccount("200000", "Member Contributions Payable","Contributions"),
            CreateMainAccount("210000", "Employer Contributions",      "Emp Contrib"),
            CreateMainAccount("300000", "Investment Portfolio",        "Investments"),
            CreateMainAccount("310000", "Investment Income",           "Inv Income"),
            CreateMainAccount("400000", "Operating Expenses",         "Expenses"),
            CreateMainAccount("410000", "Administrative Expenses",    "Admin Exp"),
            CreateMainAccount("500000", "Member Benefits Payable",    "Benefits"),
            CreateMainAccount("510000", "Withdrawal Benefits",        "Withdrawals")
        );
    }

    private static void SeedTransactionMasters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.TransactionMaster>().HasData(
            CreateTransactionMaster("PF1", "CON", "Contributions",       "C  ", "Member Contribution"),
            CreateTransactionMaster("PF1", "WIT", "Withdrawal",          "W  ", "Member Withdrawal"),
            CreateTransactionMaster("PF1", "JV1", "Journal Voucher",     "J  ", "Journal Entry"),
            CreateTransactionMaster("PF1", "DIV", "Dividend",            "D  ", "Investment Dividend"),
            CreateTransactionMaster("PF1", "TRF", "Transfer In/Out",     "T  ", "Fund Transfer"),
            CreateTransactionMaster("PF2", "CON", "Contributions",       "C  ", "Member Contribution"),
            CreateTransactionMaster("PF2", "WIT", "Withdrawal",          "W  ", "Member Withdrawal")
        );
    }

    private static void SeedPfSubAccounts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.PfSubAccount>().HasData(
            CreatePfSubAccount(1001, "Employee Contribution"),
            CreatePfSubAccount(1002, "Employer Contribution"),
            CreatePfSubAccount(1003, "Voluntary Contribution"),
            CreatePfSubAccount(2001, "Normal Withdrawal"),
            CreatePfSubAccount(2002, "Death Benefit"),
            CreatePfSubAccount(2003, "Disability Benefit"),
            CreatePfSubAccount(3001, "Fixed Deposit Return"),
            CreatePfSubAccount(3002, "Equity Dividend")
        );
    }

    // Helpers to populate private-setter entities for seed data
    private static object CreateMainAccount(string code, string name, string shortName) =>
        new { MainAccountCode = code, MainAccountName = name, MainAccountShrtName = shortName, MainClosureFlag = "N" };

    private static object CreateTransactionMaster(string trustCode, string code, string name, string type, string value) =>
        new { TransactionTrustCode = trustCode, TransactionCode = code, TransactionName = name, TransactionType = type, TransactionValue = value };

    private static object CreatePfSubAccount(long code, string name) =>
        new { SubAccCod = code, SubAccNam = name };

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        return result;
    }
}
