using InvestmentService.Domain.Entities;
using InvestmentService.Domain.Interfaces;
using InvestmentService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvestmentService.Infrastructure.Repositories;

public class InvestmentRepository : IInvestmentRepository
{
    private readonly InvestmentDbContext _db;
    public InvestmentRepository(InvestmentDbContext db) => _db = db;

    public async Task<Investment?> GetByIdAsync(long invNo, CancellationToken ct = default) =>
        await _db.Investments
            .Include(i => i.Category)
            .Include(i => i.SubCategory)
            .Include(i => i.Broker)
            .Include(i => i.SaleDetails)
            .Include(i => i.ScheduleDetails)
            .Include(i => i.ApprovalDetails)
            .FirstOrDefaultAsync(i => i.InvNo == invNo, ct);

    public async Task<IEnumerable<Investment>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Investments
            .Include(i => i.Category)
            .Include(i => i.SubCategory)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<IEnumerable<Investment>> GetActiveInvestmentsAsync(CancellationToken ct = default) =>
        await _db.Investments
            .Include(i => i.Category)
            .Where(i => i.Status == "A")
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<IEnumerable<Investment>> GetByCategoryAsync(int categoryId, CancellationToken ct = default) =>
        await _db.Investments
            .Include(i => i.Category)
            .Where(i => i.CategoryId == categoryId)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<IEnumerable<Investment>> GetMaturedInvestmentsAsync(DateTime asOfDate, CancellationToken ct = default) =>
        await _db.Investments
            .Include(i => i.Category)
            .Where(i => i.Status == "A" && i.MaturityDate <= asOfDate)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task AddAsync(Investment investment, CancellationToken ct = default) =>
        await _db.Investments.AddAsync(investment, ct);

    public Task UpdateAsync(Investment investment, CancellationToken ct = default)
    {
        _db.Investments.Update(investment);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(long invNo, CancellationToken ct = default) =>
        await _db.Investments.AnyAsync(i => i.InvNo == invNo, ct);
}

public class SaleDetailRepository : ISaleDetailRepository
{
    private readonly InvestmentDbContext _db;
    public SaleDetailRepository(InvestmentDbContext db) => _db = db;

    public async Task<SaleDetail?> GetByIdAsync(long saleNo, CancellationToken ct = default) =>
        await _db.SaleDetails.FindAsync(new object[] { saleNo }, ct);

    public async Task<IEnumerable<SaleDetail>> GetByInvestmentAsync(long invNo, CancellationToken ct = default) =>
        await _db.SaleDetails.Where(s => s.InvNo == invNo).AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(SaleDetail saleDetail, CancellationToken ct = default) =>
        await _db.SaleDetails.AddAsync(saleDetail, ct);
}

public class ScheduleDetailRepository : IScheduleDetailRepository
{
    private readonly InvestmentDbContext _db;
    public ScheduleDetailRepository(InvestmentDbContext db) => _db = db;

    public async Task<IEnumerable<ScheduleDetail>> GetByInvestmentAsync(long invNo, CancellationToken ct = default) =>
        await _db.ScheduleDetails.Where(s => s.InvNo == invNo).AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<ScheduleDetail>> GetPendingSchedulesAsync(DateTime asOfDate, CancellationToken ct = default) =>
        await _db.ScheduleDetails
            .Where(s => s.DueDate <= asOfDate && s.ReceivedAmount == null)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task AddRangeAsync(IEnumerable<ScheduleDetail> details, CancellationToken ct = default) =>
        await _db.ScheduleDetails.AddRangeAsync(details, ct);

    public Task UpdateAsync(ScheduleDetail detail, CancellationToken ct = default)
    {
        _db.ScheduleDetails.Update(detail);
        return Task.CompletedTask;
    }
}

public class CategoryRepository : ICategoryRepository
{
    private readonly InvestmentDbContext _db;
    public CategoryRepository(InvestmentDbContext db) => _db = db;

    public async Task<InvestmentCategory?> GetByIdAsync(int code, CancellationToken ct = default) =>
        await _db.Categories.Include(c => c.SubCategories).FirstOrDefaultAsync(c => c.Code == code, ct);

    public async Task<IEnumerable<InvestmentCategory>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Categories.Include(c => c.SubCategories).AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(InvestmentCategory category, CancellationToken ct = default) =>
        await _db.Categories.AddAsync(category, ct);
}

public class SubCategoryRepository : ISubCategoryRepository
{
    private readonly InvestmentDbContext _db;
    public SubCategoryRepository(InvestmentDbContext db) => _db = db;

    public async Task<InvestmentSubCategory?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _db.SubCategories.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<InvestmentSubCategory>> GetByCategoryAsync(int categoryId, CancellationToken ct = default) =>
        await _db.SubCategories.Where(s => s.CategoryId == categoryId).AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(InvestmentSubCategory subCategory, CancellationToken ct = default) =>
        await _db.SubCategories.AddAsync(subCategory, ct);
}

public class BrokerRepository : IBrokerRepository
{
    private readonly InvestmentDbContext _db;
    public BrokerRepository(InvestmentDbContext db) => _db = db;

    public async Task<Broker?> GetByIdAsync(decimal brokerId, CancellationToken ct = default) =>
        await _db.Brokers.FirstOrDefaultAsync(b => b.BrokerId == brokerId, ct);

    public async Task<IEnumerable<Broker>> GetAllActiveAsync(CancellationToken ct = default) =>
        await _db.Brokers.Where(b => b.BrokerStatus == "A").AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(Broker broker, CancellationToken ct = default) =>
        await _db.Brokers.AddAsync(broker, ct);
}

public class CreditAgencyRepository : ICreditAgencyRepository
{
    private readonly InvestmentDbContext _db;
    public CreditAgencyRepository(InvestmentDbContext db) => _db = db;

    public async Task<IEnumerable<CreditAgency>> GetAllAsync(CancellationToken ct = default) =>
        await _db.CreditAgencies.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(CreditAgency agency, CancellationToken ct = default) =>
        await _db.CreditAgencies.AddAsync(agency, ct);
}

public class CreditRatingRepository : ICreditRatingRepository
{
    private readonly InvestmentDbContext _db;
    public CreditRatingRepository(InvestmentDbContext db) => _db = db;

    public async Task<IEnumerable<CreditRating>> GetAllAsync(CancellationToken ct = default) =>
        await _db.CreditRatings.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(CreditRating rating, CancellationToken ct = default) =>
        await _db.CreditRatings.AddAsync(rating, ct);
}
