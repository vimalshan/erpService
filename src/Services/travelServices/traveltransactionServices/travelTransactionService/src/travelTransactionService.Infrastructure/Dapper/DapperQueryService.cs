using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using travelTransactionService.Application.DTOs;
using travelTransactionService.Application.Interfaces;

namespace travelTransactionService.Infrastructure.Dapper;

public class DapperQueryService : IDapperQueryService
{
    private readonly string _connectionString;

    public DapperQueryService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("TravelDb")
            ?? throw new InvalidOperationException("Connection string 'TravelDb' not found.");
    }

    public async Task<IReadOnlyList<AccountMasterDto>> GetAllAccountMastersAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                AC_COM_COD AS CompanyCode,
                AC_ED_COD AS EdCode,
                AC_ACC_COD AS AccountCode,
                AC_GRD_TYP AS GradeType,
                AC_DC_FLG AS DebitCreditFlag,
                AC_SUB_COD AS SubCode,
                AC_ACC_DES AS AccountDescription
            FROM ACC_MASTER
            ORDER BY AC_COM_COD, AC_ACC_COD
            """;

        await using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<AccountMasterDto>(sql);
        return result.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<GlCodeCombinationDto>> GetAllGlCodeCombinationsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                ROW_ID AS RowId,
                CODE_COMBINATION_ID AS CodeCombinationId,
                CHART_OF_ACCOUNTS_ID AS ChartOfAccountsId,
                CONCATENATED_SEGMENTS AS ConcatenatedSegments,
                GL_ACCOUNT_TYPE AS GlAccountType,
                ENABLED_FLAG AS EnabledFlag,
                SEGMENT1 AS Segment1,
                SEGMENT2 AS Segment2,
                SEGMENT3 AS Segment3,
                DESCRIPTION AS Description
            FROM GL_CODE_COMBINATIONS_KFV
            ORDER BY CODE_COMBINATION_ID
            """;

        await using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<GlCodeCombinationDto>(sql);
        return result.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<JvInterfaceDto>> GetAllJvInterfacesAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                CODE_COMBINATION AS CodeCombination,
                SEGMENT1 AS Segment1,
                IO AS Io,
                UNIT AS Unit
            FROM JV_INTERFACE
            ORDER BY SEGMENT1
            """;

        await using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<JvInterfaceDto>(sql);
        return result.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<JvMissingCombiCodeDto>> GetAllJvMissingCombiCodesAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                AM_AGN_NAM AS AgencyName,
                INVOICE_NUM AS InvoiceNumber,
                DESCRIPTION AS Description,
                DIST_CODE_CONCENATED AS DistCodeConcatenated,
                JV_NO AS JvNumber,
                LOG_SYSID AS LogSysId
            FROM JV_MISSING_COMBICODE
            ORDER BY JV_NO
            """;

        await using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<JvMissingCombiCodeDto>(sql);
        return result.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<BatchSubBreakupDto>> GetAllBatchSubBreakupsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                SLNO AS SlNo,
                BOK_NUM AS BookingNumber,
                COST_UNIT AS CostUnit,
                COST_CODE AS CostCode,
                PRODUCT_CODE AS ProductCode,
                SUBACCOUNT_CODE AS SubAccountCode
            FROM TRAVEL_BATCH_SUB_BREAKUP
            ORDER BY SLNO
            """;

        await using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<BatchSubBreakupDto>(sql);
        return result.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<TravelApParamsDto>> GetAllTravelApParamsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                AP_UNIT_ID AS ApUnitId,
                AP_ACCOUNT_STATUS AS AccountStatus,
                AP_ACCOUNT_CODE AS AccountCode,
                AP_CONTROLCOMBID AS ControlCombId
            FROM TRAVEL_AP_PARAMS
            ORDER BY AP_UNIT_ID
            """;

        await using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<TravelApParamsDto>(sql);
        return result.ToList().AsReadOnly();
    }

    public async Task<TravelApParamsDto?> GetTravelApParamsByIdAsync(long apUnitId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                AP_UNIT_ID AS ApUnitId,
                AP_ACCOUNT_STATUS AS AccountStatus,
                AP_ACCOUNT_CODE AS AccountCode,
                AP_CONTROLCOMBID AS ControlCombId
            FROM TRAVEL_AP_PARAMS
            WHERE AP_UNIT_ID = @ApUnitId
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<TravelApParamsDto>(sql, new { ApUnitId = apUnitId });
    }

    public async Task<IReadOnlyList<SourceHistoryDto>> GetAllSourceHistoryAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                CHANGE_DATE AS ChangeDate,
                NAME AS Name,
                TYPE AS Type,
                LINE AS Line,
                TEXT AS Text
            FROM SOURCE_HIST
            ORDER BY CHANGE_DATE DESC
            """;

        await using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<SourceHistoryDto>(sql);
        return result.ToList().AsReadOnly();
    }
}
