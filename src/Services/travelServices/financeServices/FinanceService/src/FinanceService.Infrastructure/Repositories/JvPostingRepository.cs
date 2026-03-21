using Dapper;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;

namespace FinanceService.Infrastructure.Repositories;

public class JvPostingRepository
{
    private readonly IDapperContext _dapperContext;

    public JvPostingRepository(IDapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<IEnumerable<JvPostingDto>> GetJvPostingsAsync(string? companyCode = null)
    {
        using var connection = _dapperContext.CreateConnection();
        const string sql = @"
            SELECT JVINTCODE AS JvIntCode, JVDOCNUM AS JvDocNum, JV_COM_COD AS CompanyCode,
                   JV_GRD_TYP AS GradeType, JV_ST_DAT AS StartDate, JV_ED_DAT AS EndDate,
                   JV_COMMENT AS Comment, JV_PAY_NUM AS PayNumber, JV_DATE AS JvDate
            FROM JVPOSTDET
            WHERE (@CompanyCode IS NULL OR JV_COM_COD = @CompanyCode)";
        return await connection.QueryAsync<JvPostingDto>(sql, new { CompanyCode = companyCode });
    }
}
