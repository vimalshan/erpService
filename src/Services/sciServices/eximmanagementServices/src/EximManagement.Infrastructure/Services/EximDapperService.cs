using Dapper;
using EximManagement.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EximManagement.Infrastructure.Services;

/// <summary>Dapper-based service for Stored Procedure calls and advanced queries.</summary>
public class EximDapperService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    public async Task<long> RegisterEximProductAsync(string productName, string? oracleCode, long updatedBy)
    {
        await using var conn = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@p_ProductName", productName);
        parameters.Add("@p_OracleCode", oracleCode);
        parameters.Add("@p_UpdatedBy", updatedBy);
        parameters.Add("@p_ProductID", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);

        await conn.ExecuteAsync("dbo.usp_RegisterEximProduct", parameters,
            commandType: System.Data.CommandType.StoredProcedure);

        return parameters.Get<long>("@p_ProductID");
    }

    public async Task<IEnumerable<EximDataExportDto>> GetEximExportDataAsync(DateTime start, DateTime end)
    {
        await using var conn = new SqlConnection(_connectionString);
        var sql = @"SELECT TOP 1000
            DATA_ID AS DataId, EXIM_DATE AS EximDate, HSCODE AS HsCode,
            PRODDESC AS ProdDesc, COUNTRYDEST AS CountryDest, PORTDEST AS PortDest,
            STDQTY AS StdQty, STDUNIT AS StdUnit, FOBINR AS FobInr, FOBDOL AS FobDol,
            EXP_NAME AS ExpName, IMP_NAME AS ImpName, IMP_COUNTRY AS ImpCountry,
            IEC AS Iec, SB_NO AS SbNo, EMONTH AS EMonth, FILE_ID AS FileId
            FROM dbo.EXIM_DATA_EXPORT
            WHERE EXIM_DATE BETWEEN @start AND @end
            ORDER BY EXIM_DATE DESC";

        return await conn.QueryAsync<EximDataExportDto>(sql, new { start, end });
    }

    public async Task<IEnumerable<EximDataImportDto>> GetEximImportDataAsync(DateTime start, DateTime end)
    {
        await using var conn = new SqlConnection(_connectionString);
        var sql = @"SELECT TOP 1000
            DATA_ID AS DataId, EXIM_DATE AS EximDate, HSCODE AS HsCode,
            PRODDESC AS ProdDesc, COUNTRYORG AS CountryOrg, PORTDEST AS PortDest,
            STDQTY AS StdQty, STDUNIT AS StdUnit, FOBINR AS FobInr, FOBDOL AS FobDol,
            IMP_NAME AS ImpName, EXP_NAME AS ExpName, IEC AS Iec,
            BE_NO AS BeNo, EMONTH AS EMonth, FILE_ID AS FileId
            FROM dbo.EXIM_DATA_IMPORT
            WHERE EXIM_DATE BETWEEN @start AND @end
            ORDER BY EXIM_DATE DESC";

        return await conn.QueryAsync<EximDataImportDto>(sql, new { start, end });
    }

    public async Task<IEnumerable<dynamic>> SearchEximDataAsync(string? hsCode, string? productDesc, string dataType)
    {
        await using var conn = new SqlConnection(_connectionString);
        var table = dataType.ToUpperInvariant() == "IMPORT" ? "EXIM_DATA_IMPORT" : "EXIM_DATA_EXPORT";
        var whereClause = "WHERE 1=1";
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(hsCode))
        { whereClause += " AND HSCODE = @HsCode"; parameters.Add("@HsCode", long.Parse(hsCode)); }
        if (!string.IsNullOrWhiteSpace(productDesc))
        { whereClause += " AND PRODDESC LIKE @ProdDesc"; parameters.Add("@ProdDesc", $"%{productDesc}%"); }

        var sql = $"SELECT TOP 500 * FROM dbo.{table} {whereClause} ORDER BY EXIM_DATE DESC";
        return await conn.QueryAsync(sql, parameters);
    }
}
