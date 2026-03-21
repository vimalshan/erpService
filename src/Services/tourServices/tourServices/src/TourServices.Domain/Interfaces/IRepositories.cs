using TourServices.Domain.Aggregates;
using TourServices.Domain.Entities;

namespace TourServices.Domain.Interfaces;

public interface ITourPackageRepository
{
    Task<TourPackage?> GetByIdAsync(long tourId, CancellationToken ct = default);
    Task<IEnumerable<TourPackage>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<TourPackage>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task AddAsync(TourPackage tourPackage, CancellationToken ct = default);
    void Update(TourPackage tourPackage);
    void Delete(TourPackage tourPackage);
}

public interface ITourRegistrationRepository
{
    Task<TourRegistration?> GetByIdAsync(long registrationId, CancellationToken ct = default);
    Task<IEnumerable<TourRegistration>> GetByTourIdAsync(long tourId, CancellationToken ct = default);
    Task<IEnumerable<TourRegistration>> GetByParticipantIdAsync(long participantId, CancellationToken ct = default);
    Task<int> GetActiveCountByTourAsync(long tourId, CancellationToken ct = default);
}
