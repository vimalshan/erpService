using Dapper;
using MasterDataService.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MasterDataService.Infrastructure.Dapper;

public interface IDapperQueryService
{
    Task<IEnumerable<LovMasterDto>> GetLovByCategoryDapperAsync(string category, CancellationToken cancellationToken = default);
    Task<IEnumerable<ConfigurationDto>> GetAllConfigurationsDapperAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<RateMasterDto>> GetRatesByTrustCodeDapperAsync(string trustCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<FundTypeDto>> GetAllFundTypesDapperAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<StatusMasterDto>> GetStatusByTypeDapperAsync(string statusType, CancellationToken cancellationToken = default);
}

public class DapperQueryService : IDapperQueryService
{
    private readonly string _connectionString;

    public DapperQueryService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<IEnumerable<LovMasterDto>> GetLovByCategoryDapperAsync(string category, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql = "SELECT LOV_ID AS LovId, LOV_CODE AS LovCode, LOV_DESC AS LovDescription, LOV_VALUE AS LovValue, LOV_CATEGORY AS LovCategory, LOV_STATUS AS LovStatus FROM LOV_MASTER WHERE LOV_CATEGORY = @Category AND LOV_STATUS = 'A' ORDER BY LOV_CODE";
        var result = await connection.QueryAsync<LovMasterDto>(sql, new { Category = category });
        return result;
    }

    public async Task<IEnumerable<ConfigurationDto>> GetAllConfigurationsDapperAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql = "SELECT CONFIG_ID AS ConfigId, CONFIG_KEY AS ConfigKey, CONFIG_VALUE AS ConfigValue, CONFIG_TYPE AS ConfigType, CONFIG_DESCRIPTION AS ConfigDescription, CREATED_DATE AS CreatedDate, UPDATED_DATE AS UpdatedDate, CREATED_BY AS CreatedBy FROM CONFIGURATION ORDER BY CONFIG_KEY";
        return await connection.QueryAsync<ConfigurationDto>(sql);
    }

    public async Task<IEnumerable<RateMasterDto>> GetRatesByTrustCodeDapperAsync(string trustCode, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql = "SELECT RT_TRUST_CODE AS TrustCode, RATE_ID AS RateId, RT_RATE_TYPE_CODE AS RateTypeCode, RATE_EFF_DATE AS RateEffectiveDate, RATE_CLS_DATE AS RateClosingDate, RATE_VALUE AS RateValue, RATE_DEL_FLAG AS RateDeleteFlag, RT_REWRK_STS AS ReworkStatus FROM RATE_MASTER WHERE RT_TRUST_CODE = @TrustCode";
        return await connection.QueryAsync<RateMasterDto>(sql, new { TrustCode = trustCode });
    }

    public async Task<IEnumerable<FundTypeDto>> GetAllFundTypesDapperAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql = "SELECT FUND_TYPECODE AS FundTypeCode, FUND_TYPENAME AS FundTypeName FROM FUND_TYPE_MASTER ORDER BY FUND_TYPECODE";
        return await connection.QueryAsync<FundTypeDto>(sql);
    }

    public async Task<IEnumerable<StatusMasterDto>> GetStatusByTypeDapperAsync(string statusType, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql = "SELECT STATUS_TYPE AS StatusType, STATUS_CODE AS StatusCodeValue, STATUS_NAME AS StatusName FROM STATUS_MASTER WHERE STATUS_TYPE = @StatusType ORDER BY STATUS_CODE";
        return await connection.QueryAsync<StatusMasterDto>(sql, new { StatusType = statusType });
    }
}
