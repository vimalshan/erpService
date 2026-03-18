using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace PromotionService.Infrastructure.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(object id, CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    IQueryable<T> AsQueryable();
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void Delete(T entity);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
}

public interface IUnitOfWork : IDisposable
{
    IRepository<Domain.Entities.Rating> Ratings { get; }
    IRepository<Domain.Entities.PromotionRecommendation> PromotionRecommendations { get; }
    IRepository<Domain.Entities.IncrementRequest> IncrementRequests { get; }
    IRepository<Domain.Entities.VTCAssessment> VTCAssessments { get; }
    IRepository<Domain.Entities.AppraisalAmount> AppraisalAmounts { get; }
    IRepository<Domain.Entities.CTGPromotion> CTGPromotions { get; }
    IRepository<Domain.Entities.HorizontalPromotion> HorizontalPromotions { get; }
    IRepository<Domain.Entities.DirectIncrement> DirectIncrements { get; }
    IRepository<Domain.Entities.VTCCorrection> VTCCorrections { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
