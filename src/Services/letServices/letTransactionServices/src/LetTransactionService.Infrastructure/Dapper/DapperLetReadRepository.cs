using Dapper;
using LetTransactionService.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LetTransactionService.Infrastructure.Dapper;

public class DapperLetReadRepository(IConfiguration configuration)
{
    private SqlConnection CreateConnection()
        => new(configuration.GetConnectionString("DefaultConnection"));

    public async Task<LetMainDto?> GetLetRequestDetailsAsync(long requestNumber, CancellationToken ct = default)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync(ct);

        var main = await conn.QueryFirstOrDefaultAsync<dynamic>(
            "SELECT REQ_NUM, FINYEAR_SRLNO, EMP_USERID, SUP_USERID, REQ_DATE FROM LET_MAIN WHERE REQ_NUM = @ReqNum",
            new { ReqNum = requestNumber });

        if (main is null) return null;

        var subs = await conn.QueryAsync<dynamic>(
            "SELECT * FROM LET_SUB WHERE LS_REQ_NUM = @ReqNum ORDER BY LS_SRL_NUM",
            new { ReqNum = requestNumber });

        return new LetMainDto(
            (long)main.REQ_NUM,
            (int)main.FINYEAR_SRLNO,
            (string)main.EMP_USERID,
            (string?)main.SUP_USERID,
            (DateTime?)main.REQ_DATE,
            subs.Select(s => new LetSubDto(
                (long)s.LS_REQ_NUM, (int)s.LS_SRL_NUM, (DateTime?)s.LS_MOD_DAT, (string?)s.LS_MOD_USER,
                ((string?)s.LS_PREF_MODDEV) ?? string.Empty, (string?)s.LS_ACT_TAKEN,
                (int?)s.LS_CRS_ID, (string?)s.LS_TRNPRG_BHR, (string?)s.LS_IMPBEN_PRO,
                (string?)s.LS_MEASURE_CP, (string?)s.LS_MIDYER_REVNAM, (string?)s.LS_MIDYER_REVDAT,
                (string?)s.LS_MIDYER_REVREM, (string?)s.LS_ANNYER_REVNAM, (string?)s.LS_ANNYER_REVDAT,
                (string?)s.LS_ANNYER_REVREM, (int?)s.LS_COMP_DEV, (string?)s.LS_DOMKNOW_DEV,
                (string?)s.LS_DOMKNOW_DEV_DET, (string?)s.LS_PROCES_DEV, (string?)s.LS_PROCES_DEV_DET,
                ((string?)s.LS_LETSUB_CODE) ?? string.Empty, (string?)s.LS_REV_TYPE)));
    }

    public async Task<IEnumerable<PendingReviewDto>> GetPendingReviewsAsync(CancellationToken ct = default)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync(ct);

        return await conn.QueryAsync<PendingReviewDto>(
            """
            SELECT REV_SRL_NUM AS ReviewSerialNumber, REV_FED_NUM AS FeedbackNumber,
                   REV_REM_MRK1 AS ImplementationGoal, REV_NEXT_DATE AS NextReviewDate
            FROM REVIEW_MAIN
            WHERE REV_STATUS = 'N'
            ORDER BY REV_NEXT_DATE
            """);
    }
}
