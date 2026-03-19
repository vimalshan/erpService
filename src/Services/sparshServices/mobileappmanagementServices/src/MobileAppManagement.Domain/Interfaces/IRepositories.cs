using MobileAppManagement.Domain.Entities;

namespace MobileAppManagement.Domain.Interfaces;

public interface IAppDeviceRepository
{
    Task<AppDeviceDetail?> GetByKeyAsync(decimal employeeSysId, string deviceId, CancellationToken ct = default);
    Task<IEnumerable<AppDeviceDetail>> GetByEmployeeAsync(decimal employeeSysId, CancellationToken ct = default);
    Task AddAsync(AppDeviceDetail device, CancellationToken ct = default);
    Task UpdateAsync(AppDeviceDetail device, CancellationToken ct = default);
}

public interface ILoginDetailRepository
{
    Task<LoginDetail?> GetByIdAsync(decimal loginId, CancellationToken ct = default);
    Task<IEnumerable<LoginDetail>> GetByUserAsync(decimal userSysId, CancellationToken ct = default);
    Task AddAsync(LoginDetail login, CancellationToken ct = default);
}

public interface IAppRegistrationRepository
{
    Task<AppRegistration?> GetByIdAsync(long registrationId, CancellationToken ct = default);
    Task<IEnumerable<AppRegistration>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<IEnumerable<AppRegistration>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task AddAsync(AppRegistration registration, CancellationToken ct = default);
    Task UpdateAsync(AppRegistration registration, CancellationToken ct = default);
}
