using MobileAppManagement.Application.DTOs;

namespace MobileAppManagement.Application.Interfaces;

public interface IDapperQueryService
{
    Task<IEnumerable<AppDeviceDetailDto>> GetDevicesByEmployeeAsync(decimal employeeSysId, CancellationToken ct = default);
    Task<IEnumerable<LoginDetailDto>> GetLoginsByUserAsync(decimal userSysId, CancellationToken ct = default);
    Task<string> RegisterDeviceViaProcAsync(decimal empSysId, string deviceId, char deviceType,
        string? imeiNo, decimal updatedBy, CancellationToken ct = default);
    Task<(decimal LoginId, string Message)> LogUserLoginViaProcAsync(decimal userSysId, string? deviceId,
        string? imeiNo, char? deviceType, CancellationToken ct = default);
}
