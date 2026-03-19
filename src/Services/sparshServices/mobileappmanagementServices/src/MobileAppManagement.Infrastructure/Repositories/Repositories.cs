using Microsoft.EntityFrameworkCore;
using MobileAppManagement.Domain.Entities;
using MobileAppManagement.Domain.Interfaces;
using MobileAppManagement.Infrastructure.Persistence;

namespace MobileAppManagement.Infrastructure.Repositories;

public class AppDeviceRepository(MobileAppDbContext context) : IAppDeviceRepository
{
    public async Task<AppDeviceDetail?> GetByKeyAsync(decimal employeeSysId, string deviceId, CancellationToken ct)
        => await context.AppDeviceDetails.FirstOrDefaultAsync(
            d => d.EmployeeSysId == employeeSysId && d.DeviceId == deviceId, ct);

    public async Task<IEnumerable<AppDeviceDetail>> GetByEmployeeAsync(decimal employeeSysId, CancellationToken ct)
        => await context.AppDeviceDetails
            .Where(d => d.EmployeeSysId == employeeSysId)
            .OrderByDescending(d => d.UpdatedOn)
            .ToListAsync(ct);

    public async Task AddAsync(AppDeviceDetail device, CancellationToken ct)
        => await context.AppDeviceDetails.AddAsync(device, ct);

    public Task UpdateAsync(AppDeviceDetail device, CancellationToken ct)
    {
        // Entity is already tracked by EF Core after GetByKeyAsync()
        // Calling Update() marks all properties as modified
        // Just let SaveChangesAsync() detect changes automatically
        return Task.CompletedTask;
    }
}

public class LoginDetailRepository(MobileAppDbContext context) : ILoginDetailRepository
{
    public async Task<LoginDetail?> GetByIdAsync(decimal loginId, CancellationToken ct)
        => await context.LoginDetails.FirstOrDefaultAsync(l => l.LoginId == loginId, ct);

    public async Task<IEnumerable<LoginDetail>> GetByUserAsync(decimal userSysId, CancellationToken ct)
        => await context.LoginDetails
            .Where(l => l.UserSysId == userSysId)
            .OrderByDescending(l => l.Logon)
            .ToListAsync(ct);

    public async Task AddAsync(LoginDetail login, CancellationToken ct)
        => await context.LoginDetails.AddAsync(login, ct);
}

public class AppRegistrationRepository(MobileAppDbContext context) : IAppRegistrationRepository
{
    public async Task<AppRegistration?> GetByIdAsync(long registrationId, CancellationToken ct)
        => await context.AppRegistrations.FirstOrDefaultAsync(r => r.RegistrationId == registrationId, ct);

    public async Task<IEnumerable<AppRegistration>> GetByUserIdAsync(string userId, CancellationToken ct)
        => await context.AppRegistrations.Where(r => r.UserId == userId).ToListAsync(ct);

    public async Task<IEnumerable<AppRegistration>> GetByStatusAsync(string status, CancellationToken ct)
        => await context.AppRegistrations.Where(r => r.Status == status).ToListAsync(ct);

    public async Task AddAsync(AppRegistration registration, CancellationToken ct)
        => await context.AppRegistrations.AddAsync(registration, ct);

    public Task UpdateAsync(AppRegistration registration, CancellationToken ct)
    {
        // Entity is already tracked by EF Core after GetByIdAsync()
        // Calling Update() marks all properties as modified
        // Just let SaveChangesAsync() detect changes automatically
        return Task.CompletedTask;
    }
}
