namespace TravelRequestService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ITravelRequestRepository TravelRequests { get; }
    ITravelAdvanceRepository TravelAdvances { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
