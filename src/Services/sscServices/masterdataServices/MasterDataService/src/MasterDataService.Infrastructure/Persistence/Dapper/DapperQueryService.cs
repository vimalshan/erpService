using Dapper;
using MasterDataService.Application.DTOs;
using MasterDataService.Infrastructure.Persistence.Dapper;

namespace MasterDataService.Infrastructure.Persistence.Dapper;

public interface IDapperQueryService
{
    Task<IEnumerable<LovMasterDto>> GetLovValuesByTypeAsync(string lovType, CancellationToken ct = default);
    Task<IEnumerable<HoldTypeMasterDto>> GetHoldTypesByCategoryAsync(char category, CancellationToken ct = default);
    Task<IEnumerable<ScannerMasterDto>> GetScannersByLocationAsync(long locationId, CancellationToken ct = default);
}

public class DapperQueryService(IDapperContext context) : IDapperQueryService
{
    public async Task<IEnumerable<LovMasterDto>> GetLovValuesByTypeAsync(string lovType, CancellationToken ct = default)
    {
        using var connection = context.CreateConnection();
        const string sql = "SELECT LOV_ID AS LovId, LOV_TYPE AS LovType, LOV_NAME AS LovName FROM LOV_MAST WHERE LOV_TYPE = @LovType";
        return await connection.QueryAsync<LovMasterDto>(sql, new { LovType = lovType });
    }

    public async Task<IEnumerable<HoldTypeMasterDto>> GetHoldTypesByCategoryAsync(char category, CancellationToken ct = default)
    {
        using var connection = context.CreateConnection();
        const string sql = "SELECT HOLD_ID AS HoldId, HOLD_NAME AS HoldName, HOLD_CATEGORY AS HoldCategory FROM HOLDTYPE_MAST WHERE HOLD_CATEGORY = @Category";
        return await connection.QueryAsync<HoldTypeMasterDto>(sql, new { Category = category });
    }

    public async Task<IEnumerable<ScannerMasterDto>> GetScannersByLocationAsync(long locationId, CancellationToken ct = default)
    {
        using var connection = context.CreateConnection();
        const string sql = "SELECT DEVICE_ID AS DeviceId, DEVICE_NAME AS DeviceName, DEVICE_LOCID AS DeviceLocationId, DEVICE_PATH AS DevicePath FROM SCANNER_MASTER WHERE DEVICE_LOCID = @LocationId";
        return await connection.QueryAsync<ScannerMasterDto>(sql, new { LocationId = locationId });
    }
}
