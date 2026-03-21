namespace BookingService.Domain.Interfaces;

public interface IUnitOfWork
{
    IBookRequestRepository BookRequests { get; }
    IBookConfirmationRepository BookConfirmations { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
