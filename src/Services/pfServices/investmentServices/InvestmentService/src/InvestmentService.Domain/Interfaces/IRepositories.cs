using InvestmentService.Domain.Entities;

namespace InvestmentService.Domain.Interfaces;

public interface IInvestmentRepository
{
    Task<Investment?> GetByIdAsync(long invNo, CancellationToken ct = default);
    Task<IEnumerable<Investment>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<Investment>> GetActiveInvestmentsAsync(CancellationToken ct = default);
    Task<IEnumerable<Investment>> GetByCategoryAsync(int categoryId, CancellationToken ct = default);
    Task<IEnumerable<Investment>> GetMaturedInvestmentsAsync(DateTime asOfDate, CancellationToken ct = default);
    Task AddAsync(Investment investment, CancellationToken ct = default);
    Task UpdateAsync(Investment investment, CancellationToken ct = default);
    Task<bool> ExistsAsync(long invNo, CancellationToken ct = default);
}

public interface ISaleDetailRepository
{
    Task<SaleDetail?> GetByIdAsync(long saleNo, CancellationToken ct = default);
    Task<IEnumerable<SaleDetail>> GetByInvestmentAsync(long invNo, CancellationToken ct = default);
    Task AddAsync(SaleDetail saleDetail, CancellationToken ct = default);
}

public interface IScheduleDetailRepository
{
    Task<IEnumerable<ScheduleDetail>> GetByInvestmentAsync(long invNo, CancellationToken ct = default);
    Task<IEnumerable<ScheduleDetail>> GetPendingSchedulesAsync(DateTime asOfDate, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<ScheduleDetail> details, CancellationToken ct = default);
    Task UpdateAsync(ScheduleDetail detail, CancellationToken ct = default);
}

public interface ICategoryRepository
{
    Task<InvestmentCategory?> GetByIdAsync(int code, CancellationToken ct = default);
    Task<IEnumerable<InvestmentCategory>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(InvestmentCategory category, CancellationToken ct = default);
}

public interface ISubCategoryRepository
{
    Task<InvestmentSubCategory?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<InvestmentSubCategory>> GetByCategoryAsync(int categoryId, CancellationToken ct = default);
    Task AddAsync(InvestmentSubCategory subCategory, CancellationToken ct = default);
}

public interface IBrokerRepository
{
    Task<Broker?> GetByIdAsync(decimal brokerId, CancellationToken ct = default);
    Task<IEnumerable<Broker>> GetAllActiveAsync(CancellationToken ct = default);
    Task AddAsync(Broker broker, CancellationToken ct = default);
}

public interface ICreditAgencyRepository
{
    Task<IEnumerable<CreditAgency>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(CreditAgency agency, CancellationToken ct = default);
}

public interface ICreditRatingRepository
{
    Task<IEnumerable<CreditRating>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(CreditRating rating, CancellationToken ct = default);
}

public interface IUnitOfWork : IDisposable
{
    IInvestmentRepository Investments { get; }
    ISaleDetailRepository SaleDetails { get; }
    IScheduleDetailRepository ScheduleDetails { get; }
    ICategoryRepository Categories { get; }
    ISubCategoryRepository SubCategories { get; }
    IBrokerRepository Brokers { get; }
    ICreditAgencyRepository CreditAgencies { get; }
    ICreditRatingRepository CreditRatings { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
