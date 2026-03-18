using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using EmployeeRelations.Application.DTOs;

namespace EmployeeRelations.Infrastructure.Persistence.Dapper;

public interface IDapperReadRepository
{
    Task<IEnumerable<EwsMainDto>> GetEwsDashboardAsync(int periodNo, CancellationToken ct = default);
    Task<IEnumerable<SurveyMasterDto>> GetActiveSurveysAsync(CancellationToken ct = default);
    Task<IEnumerable<DisciplinaryMainDto>> GetDisciplinaryCasesByUnitAsync(long unitId, CancellationToken ct = default);
}

public class DapperReadRepository : IDapperReadRepository
{
    private readonly string _connectionString;

    public DapperReadRepository(string connectionString) => _connectionString = connectionString;

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<EwsMainDto>> GetEwsDashboardAsync(int periodNo, CancellationToken ct = default)
    {
        const string sql = @"
            SELECT EWS_ID AS Id, EWS_EMPSYSID AS EmpSysId, EWS_PERIODNO AS PeriodNo,
                   EWS_STATUS AS Status, EWS_HRFLAG AS HrFlag, EWS_HRREMARKS AS HrRemarks,
                   EWS_APRFLAG AS AprFlag, EWS_APRREMARKS AS AprRemarks,
                   EWS_FINAL AS Final, EWS_HRENTRYDATE AS HrEntryDate
            FROM EWS_MAIN
            WHERE EWS_PERIODNO = @PeriodNo";

        using var conn = CreateConnection();
        var result = await conn.QueryAsync<EwsMainDto>(sql, new { PeriodNo = periodNo });
        return result;
    }

    public async Task<IEnumerable<SurveyMasterDto>> GetActiveSurveysAsync(CancellationToken ct = default)
    {
        const string sql = @"
            SELECT SURVEY_ID AS Id, SURVEY_NAME AS Name, SURVEY_IMAGE AS Image,
                   SURVEY_STARTDATE AS StartDate, SURVEY_ENDDATE AS EndDate,
                   SURVEY_CLSDATE AS ClosureDate, SURVEY_AUTOLOCK AS AutoLock,
                   SURVEY_FLAG AS Flag, SURVEY_TEMPLATEID AS TemplateId
            FROM SURVEY_MASTER
            WHERE SURVEY_FLAG IS NULL OR SURVEY_FLAG <> 'C'";

        using var conn = CreateConnection();
        var result = await conn.QueryAsync<SurveyMasterDto>(new CommandDefinition(sql, cancellationToken: ct));
        return result.Select(s => s with { Questions = Enumerable.Empty<SurveyQuestionDto>() });
    }

    public async Task<IEnumerable<DisciplinaryMainDto>> GetDisciplinaryCasesByUnitAsync(long unitId, CancellationToken ct = default)
    {
        const string sql = @"
            SELECT DISCIPLINE_MAINID AS Id, DISCIPLINE_UNITID AS UnitId,
                   DISCIPLINE_DATE AS Date, DISCIPLINE_DETAILS AS Details,
                   DISCIPLINE_CREATEDBY AS CreatedBy, DISCIPLINE_CREATEDON AS CreatedOn
            FROM DISCIPLINARY_MAIN
            WHERE DISCIPLINE_UNITID = @UnitId
            ORDER BY DISCIPLINE_DATE DESC";

        using var conn = CreateConnection();
        var result = await conn.QueryAsync<DisciplinaryMainDto>(
            new CommandDefinition(sql, new { UnitId = unitId }, cancellationToken: ct));
        return result.Select(d => d with { Employees = Enumerable.Empty<DisciplinaryEmpDto>(), Actions = Enumerable.Empty<DisciplinaryActionDto>() });
    }
}
