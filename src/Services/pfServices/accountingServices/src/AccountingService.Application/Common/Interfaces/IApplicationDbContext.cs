using AccountingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AccountDetail> AccountDetails { get; }
    DbSet<AccountLookup> AccountLookups { get; }
    DbSet<MainAccount> MainAccounts { get; }
    DbSet<TransactionDetail> TransactionDetails { get; }
    DbSet<TransactionMaster> TransactionMasters { get; }
    DbSet<PfSubAccount> PfSubAccounts { get; }
    DbSet<GlPosting> GlPostings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
