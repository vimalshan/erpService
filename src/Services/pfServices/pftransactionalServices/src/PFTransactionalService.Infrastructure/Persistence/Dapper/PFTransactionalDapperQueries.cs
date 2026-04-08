using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PFTransactionalService.Application.DTOs;

namespace PFTransactionalService.Infrastructure.Persistence.Dapper;

public class PFTransactionalDapperQueries
{
    private readonly string _connectionString;

    public PFTransactionalDapperQueries(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PFTransactionalDb")
            ?? throw new InvalidOperationException("Connection string 'PFTransactionalDb' not found.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<PFAccumulationDto>> GetAccumulationSummaryAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                PF_ACC_ID AS PfAccId,
                EMP_SYS_ID AS EmpSysId,
                MEMBER_NO AS MemberNo,
                TRUST_CODE AS TrustCode,
                PF_ACC_BAL AS PfAccBal,
                PF_EMP_CONT_TOTAL AS PfEmpContTotal,
                PF_ER_CONT_TOTAL AS PfErContTotal,
                PF_VOL_CONT_TOTAL AS PfVolContTotal,
                PF_ACC_STATUS AS PfAccStatus,
                CREATED_ON AS CreatedOn
            FROM PF_ACCUMULATION
            WHERE PF_ACC_STATUS = 'A'
            ORDER BY CREATED_ON DESC
            """;

        using var connection = CreateConnection();
        var results = await connection.QueryAsync<PFAccumulationDto>(new CommandDefinition(sql, cancellationToken: cancellationToken));
        return results;
    }

    public async Task<PFAccumulationDto?> GetAccumulationByEmpSysIdAsync(long empSysId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                PF_ACC_ID AS PfAccId,
                EMP_SYS_ID AS EmpSysId,
                MEMBER_NO AS MemberNo,
                TRUST_CODE AS TrustCode,
                PF_ACC_BAL AS PfAccBal,
                PF_EMP_CONT_TOTAL AS PfEmpContTotal,
                PF_ER_CONT_TOTAL AS PfErContTotal,
                PF_VOL_CONT_TOTAL AS PfVolContTotal,
                PF_ACC_STATUS AS PfAccStatus,
                CREATED_BY AS CreatedBy,
                CREATED_ON AS CreatedOn,
                UPDATED_BY AS UpdatedBy,
                UPDATED_ON AS UpdatedOn
            FROM PF_ACCUMULATION
            WHERE EMP_SYS_ID = @EmpSysId
            """;

        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<PFAccumulationDto>(
            new CommandDefinition(sql, new { EmpSysId = empSysId }, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<ContributionTxnDto>> GetContributionsByEmpSysIdAsync(long empSysId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                PF_TXN_ID AS PfTxnId,
                EMP_SYS_ID AS EmpSysId,
                PF_EMP_CONTRIBUTION AS PfEmpContribution,
                PF_ER_CONTRIBUTION AS PfErContribution,
                PF_VOL_CONTRIBUTION AS PfVolContribution,
                PF_TXN_DATE AS PfTxnDate,
                PF_TXN_MONTH AS PfTxnMonth,
                PF_TXN_STATUS AS PfTxnStatus,
                CREATED_BY AS CreatedBy,
                CREATED_ON AS CreatedOn
            FROM PF_CONTRIBUTION_TXN
            WHERE EMP_SYS_ID = @EmpSysId
            ORDER BY PF_TXN_MONTH DESC
            """;

        using var connection = CreateConnection();
        return await connection.QueryAsync<ContributionTxnDto>(
            new CommandDefinition(sql, new { EmpSysId = empSysId }, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<FinancialYearDto>> GetFinancialYearsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                AC_SRL_NUM AS AcSrlNum,
                AC_STR_DAT AS AcStrDat,
                AC_END_DAT AS AcEndDat,
                AC_CLS_FLG AS AcClsFlg,
                AC_REMARKS AS AcRemarks,
                AC_INT_FLG AS AcIntFlg
            FROM COMP_FINYEAR
            ORDER BY AC_STR_DAT DESC
            """;

        using var connection = CreateConnection();
        return await connection.QueryAsync<FinancialYearDto>(new CommandDefinition(sql, cancellationToken: cancellationToken));
    }
}
