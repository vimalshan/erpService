namespace MobileAppManagement.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IAppDeviceRepository AppDevices { get; }
    ILoginDetailRepository LoginDetails { get; }
    IAppRegistrationRepository AppRegistrations { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
