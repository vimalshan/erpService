using PurchaseSalesService.Domain.Entities;

namespace PurchaseSalesService.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    Microsoft.EntityFrameworkCore.DbSet<PurchaseDetail> PurchaseDetails { get; }
    Microsoft.EntityFrameworkCore.DbSet<SaleMain> SaleMains { get; }
    Microsoft.EntityFrameworkCore.DbSet<SaleSub> SaleSubs { get; }
    Microsoft.EntityFrameworkCore.DbSet<LogPurchaseDetail> LogPurchaseDetails { get; }
    Microsoft.EntityFrameworkCore.DbSet<LogSaleMain> LogSaleMains { get; }
    Microsoft.EntityFrameworkCore.DbSet<LogSaleSub> LogSaleSubs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
