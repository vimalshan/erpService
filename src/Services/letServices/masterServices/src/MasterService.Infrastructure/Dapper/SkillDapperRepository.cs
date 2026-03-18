using Dapper;
using MasterService.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MasterService.Infrastructure.Dapper;

/// <summary>Executes stored procedures via Dapper for read-heavy operations.</summary>
public sealed class SkillDapperRepository(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<IEnumerable<SkillDto>> GetSkillsAsync(char? skillType = null)
    {
        await using var connection = new SqlConnection(_connectionString);
        var results = await connection.QueryAsync(
            "dbo.usp_Master_GetSkills",
            new { p_SkillType = skillType },
            commandType: System.Data.CommandType.StoredProcedure);

        return results.Select(r => new SkillDto(
            (long)r.SK_SKL_COD, (string)r.SK_SKL_NAM, ((string)r.SK_SKL_TYP)[0],
            (decimal?)r.SK_WGT_NUM, null, (DateTime?)r.SK_EFF_DAT, (DateTime?)r.SK_CLS_DAT,
            r.SK_CLS_DAT is null));
    }

    public async Task<IEnumerable<TrainingProviderDto>> GetTrainingsAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        var results = await connection.QueryAsync(
            "dbo.usp_Master_GetTrainings",
            commandType: System.Data.CommandType.StoredProcedure);

        return results.Select(r => new TrainingProviderDto(
            (long)r.TR_TRN_COD, (string)r.TR_TRN_NAM, (string?)r.TR_TRN_ADD1,
            (string?)r.TR_CNT_NAM1, (string?)r.TR_PHN_NUM1, null,
            null, null, (DateTime?)r.TR_EFF_DAT, true));
    }

    public async Task<IEnumerable<JobMasterDto>> GetJobsAsync(string? categoryCode = null)
    {
        await using var connection = new SqlConnection(_connectionString);
        var results = await connection.QueryAsync(
            "dbo.usp_Master_GetJobs",
            new { p_CategoryCode = categoryCode },
            commandType: System.Data.CommandType.StoredProcedure);

        return results.Select(r => new JobMasterDto(
            (long)r.JB_JOB_COD, (string)r.JB_JOB_NAM, (string)r.JB_CAT_COD, null));
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        var results = await connection.QueryAsync(
            "dbo.usp_Master_GetCategories",
            commandType: System.Data.CommandType.StoredProcedure);

        return results.Select(r => new CategoryDto(
            (string)r.CT_CAT_COD, (string)r.CT_CAT_NAM, (long?)r.CT_SRL_NUM));
    }
}
