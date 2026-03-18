using InvestmentService.Domain.Interfaces;
using InvestmentService.Infrastructure.Data;

namespace InvestmentService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly InvestmentDbContext _db;

    public UnitOfWork(InvestmentDbContext db)
    {
        _db = db;
        Investments = new InvestmentRepository(db);
        SaleDetails = new SaleDetailRepository(db);
        ScheduleDetails = new ScheduleDetailRepository(db);
        Categories = new CategoryRepository(db);
        SubCategories = new SubCategoryRepository(db);
        Brokers = new BrokerRepository(db);
        CreditAgencies = new CreditAgencyRepository(db);
        CreditRatings = new CreditRatingRepository(db);
    }

    public IInvestmentRepository Investments { get; }
    public ISaleDetailRepository SaleDetails { get; }
    public IScheduleDetailRepository ScheduleDetails { get; }
    public ICategoryRepository Categories { get; }
    public ISubCategoryRepository SubCategories { get; }
    public IBrokerRepository Brokers { get; }
    public ICreditAgencyRepository CreditAgencies { get; }
    public ICreditRatingRepository CreditRatings { get; }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _db.SaveChangesAsync(ct);

    public void Dispose() => _db.Dispose();
}
