using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using CardManagement.Application.Common.DTOs;

namespace CardManagement.Infrastructure.Persistence.Dapper;

public interface ICardDapperRepository
{
    Task<IEnumerable<GuestCardDto>> SearchCardsAsync(string? cardNumber, long? canteenUnit, CancellationToken ct = default);
    Task<IEnumerable<CardSettlementDto>> GetSettlementHistoryAsync(long canteenUnit, CancellationToken ct = default);
}

public class CardDapperRepository : ICardDapperRepository
{
    private readonly string _connectionString;

    public CardDapperRepository(string connectionString) => _connectionString = connectionString;

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<GuestCardDto>> SearchCardsAsync(string? cardNumber, long? canteenUnit, CancellationToken ct = default)
    {
        const string sql = """
            SELECT 
                GC_COM_COD AS CanteenUnit,
                GC_CRD_SEQ AS CardSequence,
                GC_CRD_NUM AS CardNumber,
                GC_CRD_NAM AS CardName,
                GC_REP_UNT AS ReportingUnit,
                GC_CRD_DEP AS ReportingDepartment,
                GC_CRD_TYP AS CardType,
                GC_EFF_DAT AS EffectiveDate,
                GC_CLS_DAT AS ClosingDate,
                CASE WHEN GC_CLS_DAT IS NULL OR GC_CLS_DAT > GETUTCDATE() THEN 1 ELSE 0 END AS IsActive
            FROM GUEST_CARD_MASTER
            WHERE (@CardNumber IS NULL OR GC_CRD_NUM = @CardNumber)
              AND (@CanteenUnit IS NULL OR GC_COM_COD = @CanteenUnit)
            ORDER BY GC_CRD_SEQ
            """;

        using var conn = CreateConnection();
        var results = await conn.QueryAsync<GuestCardDto>(
            new CommandDefinition(sql, new { CardNumber = cardNumber, CanteenUnit = canteenUnit }, cancellationToken: ct));
        return results;
    }

    public async Task<IEnumerable<CardSettlementDto>> GetSettlementHistoryAsync(long canteenUnit, CancellationToken ct = default)
    {
        const string sql = """
            SELECT 
                ST_SYSID AS SysId,
                ST_CAN_UNT AS CanteenUnit,
                ST_CRD_NUM AS CardNumber,
                ST_SET_DAT AS SettlementDate,
                ST_UPD_DAT AS UpdatedDate
            FROM CARD_SETTLEMENT
            WHERE ST_CAN_UNT = @CanteenUnit
            ORDER BY ST_SET_DAT DESC
            """;

        using var conn = CreateConnection();
        var results = await conn.QueryAsync<CardSettlementDto>(
            new CommandDefinition(sql, new { CanteenUnit = canteenUnit }, cancellationToken: ct));
        return results;
    }
}
