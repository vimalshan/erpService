using MobileAppManagement.Domain.Interfaces;
using MobileAppManagement.Infrastructure.Persistence;

namespace MobileAppManagement.Infrastructure.Repositories;

public class UnitOfWork(MobileAppDbContext context,
    IAppDeviceRepository appDevices,
    ILoginDetailRepository loginDetails,
    IAppRegistrationRepository appRegistrations) : IUnitOfWork
{
    public IAppDeviceRepository AppDevices { get; } = appDevices;
    public ILoginDetailRepository LoginDetails { get; } = loginDetails;
    public IAppRegistrationRepository AppRegistrations { get; } = appRegistrations;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await context.SaveChangesAsync(ct);

    public void Dispose() => context.Dispose();
}
