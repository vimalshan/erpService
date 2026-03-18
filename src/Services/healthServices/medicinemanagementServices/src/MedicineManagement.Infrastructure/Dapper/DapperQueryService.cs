using Dapper;
using MedicineManagement.Application.DTOs;
using Microsoft.Data.SqlClient;

namespace MedicineManagement.Infrastructure.Dapper;

public class DapperQueryService(string connectionString)
{
    public async Task<IEnumerable<StockSummaryDto>> GetStockSummaryAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT m.MM_MED_COD AS MedicineCode, m.MM_MED_NAM AS MedicineName,
                   ISNULL(SUM(CASE WHEN c.MD_REC_TYP IN ('O','P') THEN c.MD_MED_QNT ELSE 0 END), 0) -
                   ISNULL(SUM(CASE WHEN c.MD_REC_TYP IN ('I','E') THEN c.MD_MED_QNT ELSE 0 END), 0) AS CurrentStock,
                   m.MM_ORD_MIN AS MinLevel, m.MM_ORD_MAX AS MaxLevel
            FROM MEDICINE_MAST m
            LEFT JOIN MEDICINE_CREDIT c ON m.MM_MED_COD = c.MD_MED_COD AND (c.MD_CAN_FLG IS NULL OR c.MD_CAN_FLG <> 'Y')
            GROUP BY m.MM_MED_COD, m.MM_MED_NAM, m.MM_ORD_MIN, m.MM_ORD_MAX
            """;

        await using var connection = new SqlConnection(connectionString);
        return await connection.QueryAsync<StockSummaryDto>(new CommandDefinition(sql, cancellationToken: ct));
    }

    public async Task<IEnumerable<MedicineCreditDto>> GetTransactionHistoryAsync(string medicineCode, DateTime from, DateTime to, CancellationToken ct = default)
    {
        const string sql = """
            SELECT MD_COM_COD AS CompanyCode, MD_TRN_COD AS TransactionCode,
                   MD_MED_COD AS MedicineCode, MD_REC_TYP AS RecordType,
                   MD_MED_QNT AS Quantity, MD_TRN_DAT AS TransactionDate,
                   MD_LOT_NUM AS LotNumber, MD_CAN_FLG AS CancelFlag
            FROM MEDICINE_CREDIT
            WHERE MD_MED_COD = @MedicineCode AND MD_TRN_DAT BETWEEN @From AND @To
            ORDER BY MD_TRN_DAT DESC
            """;

        await using var connection = new SqlConnection(connectionString);
        return await connection.QueryAsync<MedicineCreditDto>(
            new CommandDefinition(sql, new { MedicineCode = medicineCode, From = from, To = to }, cancellationToken: ct));
    }
}
