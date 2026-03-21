using Dapper;
using Microsoft.Data.SqlClient;
using BookingService.Application.DTOs;

namespace BookingService.Infrastructure.Persistence;

public class DapperBookingQuery(string connectionString)
{
    public async Task<IEnumerable<BookRequestMainDto>> GetBookingsSummaryAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT 
                BOOKMAIN_ID AS BookMainId,
                BOOKMAIN_TPSTATUS AS TpStatus,
                BOOKMAIN_EMPSYSID AS EmployeeSysId,
                BOOKMAIN_TYPE AS Type,
                BOOKMAIN_APPSTATUS AS ApprovalStatus,
                BOOKMAIN_CNFSTATUS AS ConfirmationStatus,
                BOOKMAIN_LASTMODIFIEDON AS LastModifiedOn
            FROM BOOKREQUEST_MAIN
            ORDER BY BOOKMAIN_LASTMODIFIEDON DESC
            """;

        await using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryAsync<BookRequestMainDto>(new CommandDefinition(sql, cancellationToken: ct));
        return result;
    }

    public async Task<BookRequestMainDto?> GetBookingDetailAsync(string bookingId, CancellationToken ct = default)
    {
        const string sql = """
            SELECT 
                BOOKMAIN_ID AS BookMainId,
                BOOKMAIN_TPSTATUS AS TpStatus,
                BOOKMAIN_TPID AS TpId,
                BOOKMAIN_EMPSYSID AS EmployeeSysId,
                BOOKMAIN_THROUGH AS Through,
                BOOKMAIN_ADMINID AS AdminId,
                BOOKMAIN_REMARKS AS Remarks,
                BOOKMAIN_TYPE AS Type,
                BOOKMAIN_APPSTATUS AS ApprovalStatus,
                BOOKMAIN_CNFSTATUS AS ConfirmationStatus,
                BOOKMAIN_PROOF AS ProofType,
                BOOKMAIN_FOODPREF AS FoodPreference,
                BOOKMAIN_BUDCOST AS BudgetedCost,
                BOOKMAIN_ENTBY AS EnteredBy,
                BOOKMAIN_ENTON AS EnteredOn,
                BOOKMAIN_LASTMODIFIEDON AS LastModifiedOn
            FROM BOOKREQUEST_MAIN
            WHERE BOOKMAIN_ID = @BookingId
            """;

        await using var connection = new SqlConnection(connectionString);
        return await connection.QueryFirstOrDefaultAsync<BookRequestMainDto>(
            new CommandDefinition(sql, new { BookingId = bookingId }, cancellationToken: ct));
    }
}
