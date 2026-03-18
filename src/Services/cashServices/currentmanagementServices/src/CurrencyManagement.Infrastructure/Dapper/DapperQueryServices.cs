using CurrencyManagement.Application.Common.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CurrencyManagement.Infrastructure.Dapper;

/// <summary>
/// Dapper-based query service for read-optimized currency queries
/// </summary>
public class CurrencyQueryService : ICurrencyQueryService
{
    private readonly string _connectionString;

    public CurrencyQueryService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public async Task<dynamic?> GetCurrencyByIdAsync(long currencyId, CancellationToken cancellationToken = default)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            return await connection.QueryFirstOrDefaultAsync(
                "SELECT CURR_ID as CurrencyId, CURR_NAME as Name, CURR_SYMBOL as Symbol FROM DEAL_CURRMAST WHERE CURR_ID = @CurrencyId",
                new { CurrencyId = currencyId });
        }
    }

    public async Task<IList<dynamic>> GetAllCurrenciesAsync(CancellationToken cancellationToken = default)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var result = await connection.QueryAsync(
                "SELECT CURR_ID as CurrencyId, CURR_NAME as Name, CURR_SYMBOL as Symbol FROM DEAL_CURRMAST ORDER BY CURR_MODIFIEDON DESC");
            return result.ToList();
        }
    }

    public async Task<dynamic?> GetCurrencyByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            return await connection.QueryFirstOrDefaultAsync(
                "SELECT CURR_ID as CurrencyId, CURR_NAME as Name, CURR_SYMBOL as Symbol FROM DEAL_CURRMAST WHERE CURR_NAME = @Name",
                new { Name = name });
        }
    }
}

/// <summary>
/// Dapper-based query service for read-optimized exchange rate queries
/// </summary>
public class ExchangeRateQueryService : IExchangeRateQueryService
{
    private readonly string _connectionString;

    public ExchangeRateQueryService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public async Task<dynamic?> GetLatestRateAsync(long fromCurrencyId, long toCurrencyId, CancellationToken cancellationToken = default)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            return await connection.QueryFirstOrDefaultAsync(
                @"SELECT TOP 1 CURRATE_ID as RateId, CURRATE_FINYEAR as FinancialYear, CURRATE_MONTH as Month,
                        CURRATE_FROMCUR as FromCurrencyId, CURRATE_TOCUR as ToCurrencyId, CURRATE_RATE as Rate
                  FROM DEAL_CURRATES
                  WHERE CURRATE_FROMCUR = @FromCurrencyId AND CURRATE_TOCUR = @ToCurrencyId
                  ORDER BY CURRATE_FINYEAR DESC, CURRATE_MONTH DESC",
                new { FromCurrencyId = fromCurrencyId, ToCurrencyId = toCurrencyId });
        }
    }

    public async Task<dynamic?> GetRateByPeriodAsync(long fromCurrencyId, long toCurrencyId, long financialYear, long month, CancellationToken cancellationToken = default)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            return await connection.QueryFirstOrDefaultAsync(
                @"SELECT CURRATE_ID as RateId, CURRATE_FINYEAR as FinancialYear, CURRATE_MONTH as Month,
                        CURRATE_FROMCUR as FromCurrencyId, CURRATE_TOCUR as ToCurrencyId, CURRATE_RATE as Rate
                  FROM DEAL_CURRATES
                  WHERE CURRATE_FROMCUR = @FromCurrencyId AND CURRATE_TOCUR = @ToCurrencyId
                    AND CURRATE_FINYEAR = @FinancialYear AND CURRATE_MONTH = @Month",
                new { FromCurrencyId = fromCurrencyId, ToCurrencyId = toCurrencyId, FinancialYear = financialYear, Month = month });
        }
    }

    public async Task<IList<dynamic>> GetRatesByPeriodAsync(long financialYear, long month, CancellationToken cancellationToken = default)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var result = await connection.QueryAsync(
                @"SELECT CURRATE_ID as RateId, CURRATE_FINYEAR as FinancialYear, CURRATE_MONTH as Month,
                        CURRATE_FROMCUR as FromCurrencyId, CURRATE_TOCUR as ToCurrencyId, CURRATE_RATE as Rate
                  FROM DEAL_CURRATES
                  WHERE CURRATE_FINYEAR = @FinancialYear AND CURRATE_MONTH = @Month
                  ORDER BY CURRATE_FROMCUR, CURRATE_TOCUR",
                new { FinancialYear = financialYear, Month = month });
            return result.ToList();
        }
    }
}
