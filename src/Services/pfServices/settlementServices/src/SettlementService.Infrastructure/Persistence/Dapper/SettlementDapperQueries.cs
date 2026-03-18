using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SettlementService.Application.DTOs;

namespace SettlementService.Infrastructure.Persistence.Dapper;

public class SettlementDapperQueries
{
    private readonly string _connectionString;

    public SettlementDapperQueries(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SettlementDb")
            ?? throw new InvalidOperationException("Connection string 'SettlementDb' not found.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<SettlementDto>> GetSettlementSummaryAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                ST_SET_NUM AS SettlementNumber,
                ST_MEMBER_NO AS MemberNo,
                ST_SET_TYPE AS SettlementType,
                ST_SETTLEMENT_AMOUNT AS SettlementAmount,
                ST_SET_DATE AS SettlementDate,
                ST_STATUS AS Status
            FROM SET_MAIN
            WHERE ST_STATUS IN ('P', 'A', 'C')
            ORDER BY ST_SET_DATE DESC
            """;

        using var connection = CreateConnection();
        var results = await connection.QueryAsync<SettlementDto>(new CommandDefinition(sql, cancellationToken: cancellationToken));
        return results;
    }

    public async Task<SettlementDto?> GetSettlementByIdAsync(long settlementNumber, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                ST_SET_NUM AS SettlementNumber,
                ST_TRUST_CODE AS TrustCode,
                ST_MEMBER_NO AS MemberNo,
                ST_SET_TYPE AS SettlementType,
                ST_SET_DATE AS SettlementDate,
                ST_DOL_DAT AS DolDate,
                ST_REASON AS Reason,
                ST_UPDON AS UpdatedOn,
                ST_UPDBY_EMP_SYSID AS UpdatedByEmpSysId,
                ST_ACC_DATE AS AccountDate,
                ST_FINYEAR AS FinYear,
                ST_JV_VOUCHER_TYPE AS JvVoucherType,
                ST_JV_NO AS JvNo,
                ST_SET_INT_FLG AS SetIntFlag,
                ST_TAXSTS AS TaxStatus,
                ST_TAXRATE AS TaxRate,
                ST_SETTLEMENT_AMOUNT AS SettlementAmount,
                ST_STATUS AS Status
            FROM SET_MAIN
            WHERE ST_SET_NUM = @SettlementNumber
            """;

        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<SettlementDto>(
            new CommandDefinition(sql, new { SettlementNumber = settlementNumber }, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<SettlementDto>> GetSettlementsByMemberAsync(long memberNo, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                ST_SET_NUM AS SettlementNumber,
                ST_MEMBER_NO AS MemberNo,
                ST_SET_TYPE AS SettlementType,
                ST_SETTLEMENT_AMOUNT AS SettlementAmount,
                ST_SET_DATE AS SettlementDate,
                ST_STATUS AS Status
            FROM SET_MAIN
            WHERE ST_MEMBER_NO = @MemberNo
            ORDER BY ST_SET_DATE DESC
            """;

        using var connection = CreateConnection();
        return await connection.QueryAsync<SettlementDto>(
            new CommandDefinition(sql, new { MemberNo = memberNo }, cancellationToken: cancellationToken));
    }
}
