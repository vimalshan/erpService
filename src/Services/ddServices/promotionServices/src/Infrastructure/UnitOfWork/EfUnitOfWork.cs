using MediatR;
using PromotionService.Domain.Entities;
using PromotionService.Infrastructure.Persistence;
using PromotionService.Infrastructure.Repositories;

namespace PromotionService.Infrastructure.UnitOfWork;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly PromotionDbContext _context;
    private readonly IPublisher _publisher;
    private bool _disposed;

    public IRepository<Rating> Ratings { get; }
    public IRepository<PromotionRecommendation> PromotionRecommendations { get; }
    public IRepository<IncrementRequest> IncrementRequests { get; }
    public IRepository<VTCAssessment> VTCAssessments { get; }
    public IRepository<AppraisalAmount> AppraisalAmounts { get; }
    public IRepository<CTGPromotion> CTGPromotions { get; }
    public IRepository<HorizontalPromotion> HorizontalPromotions { get; }
    public IRepository<DirectIncrement> DirectIncrements { get; }
    public IRepository<VTCCorrection> VTCCorrections { get; }

    public EfUnitOfWork(PromotionDbContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
        Ratings = new EfRepository<Rating>(context);
        PromotionRecommendations = new EfRepository<PromotionRecommendation>(context);
        IncrementRequests = new EfRepository<IncrementRequest>(context);
        VTCAssessments = new EfRepository<VTCAssessment>(context);
        AppraisalAmounts = new EfRepository<AppraisalAmount>(context);
        CTGPromotions = new EfRepository<CTGPromotion>(context);
        HorizontalPromotions = new EfRepository<HorizontalPromotion>(context);
        DirectIncrements = new EfRepository<DirectIncrement>(context);
        VTCCorrections = new EfRepository<VTCCorrection>(context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var result = await _context.SaveChangesAsync(ct);
        await DispatchDomainEventsAsync(ct);
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var domainEventEntities = new List<IReadOnlyList<object>>();

        foreach (var entry in _context.ChangeTracker.Entries<Rating>())
            domainEventEntities.Add(entry.Entity.DomainEvents);
        foreach (var entry in _context.ChangeTracker.Entries<PromotionRecommendation>())
            domainEventEntities.Add(entry.Entity.DomainEvents);
        foreach (var entry in _context.ChangeTracker.Entries<IncrementRequest>())
            domainEventEntities.Add(entry.Entity.DomainEvents);

        foreach (var events in domainEventEntities)
            foreach (var @event in events)
                await _publisher.Publish(@event, ct);

        foreach (var entry in _context.ChangeTracker.Entries<Rating>())
            entry.Entity.ClearDomainEvents();
        foreach (var entry in _context.ChangeTracker.Entries<PromotionRecommendation>())
            entry.Entity.ClearDomainEvents();
        foreach (var entry in _context.ChangeTracker.Entries<IncrementRequest>())
            entry.Entity.ClearDomainEvents();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
            _context.Dispose();
        _disposed = true;
    }

    public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
}
