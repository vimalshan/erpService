using Dapper;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;

namespace FinanceService.Infrastructure.Repositories;

public class PaymentRepository
{
    private readonly IDapperContext _dapperContext;

    public PaymentRepository(IDapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsAsync(string? unitCode = null)
    {
        using var connection = _dapperContext.CreateConnection();
        const string sql = @"
            SELECT AC_TRN_NUM AS TransactionNumber, AC_UNT_COD AS UnitCode,
                   AC_DC_FLG AS DebitCreditFlag, AC_TRN_AMT AS TransactionAmount,
                   AC_REM_MRK AS Remarks, AC_ACC_TYP AS AccountType,
                   AC_JV_STS AS JvPostingStatus
            FROM TRAVEL_ACCOUNT
            WHERE (@UnitCode IS NULL OR AC_UNT_COD = @UnitCode)";
        return await connection.QueryAsync<PaymentDto>(sql, new { UnitCode = unitCode });
    }
}
