using Dapper;

namespace CanteenUnit.Infrastructure.Dapper;

public class CanteenUnitDapperRepository
{
    private readonly IDapperContext _dapper;
    public CanteenUnitDapperRepository(IDapperContext dapper) => _dapper = dapper;

    public async Task<IEnumerable<dynamic>> GetUnitsWithAccessCountAsync()
    {
        const string sql = """
            SELECT u.UN_COM_COD, u.UN_UNT_NAME, u.UNT_UNT_REF,
                   COUNT(a.UN_UNT_ACC) AS AccessCount
            FROM CANTEEN_UNIT_MASTER u
            LEFT JOIN CANTEEN_UNIT_ACCESS a ON a.UN_COM_COD = u.UN_COM_COD
                                           AND a.UN_CLS_DAT IS NULL
            GROUP BY u.UN_COM_COD, u.UN_UNT_NAME, u.UNT_UNT_REF
            """;

        using var conn = _dapper.CreateConnection();
        return await conn.QueryAsync(sql);
    }

    public async Task<IEnumerable<dynamic>> SearchUnitsAsync(string? nameFilter)
    {
        const string sql = """
            SELECT UN_COM_COD, UN_UNT_NAME, UNT_UNT_REF, UN_MAX_VAL, IN_MIN_VAL
            FROM CANTEEN_UNIT_MASTER
            WHERE (@Name IS NULL OR UN_UNT_NAME LIKE '%' + @Name + '%')
            ORDER BY UN_UNT_NAME
            """;

        using var conn = _dapper.CreateConnection();
        return await conn.QueryAsync(sql, new { Name = nameFilter });
    }
}
