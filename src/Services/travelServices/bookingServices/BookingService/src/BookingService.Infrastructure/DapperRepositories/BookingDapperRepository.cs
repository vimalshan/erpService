using BookingService.Application.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BookingService.Infrastructure.DapperRepositories;

/// <summary>
/// Read-side repository using Dapper for high-performance queries.
/// </summary>
public interface IBookingReadRepository
{
    Task<BookingRequestDto?> GetBookingDetailsAsync(long bookingNumber, CancellationToken ct = default);
    Task<BookingConfirmationDto?> GetConfirmationDetailsAsync(long confirmationNumber, CancellationToken ct = default);
    Task<IEnumerable<BookingListDto>> GetUserBookingsAsync(string userCode, int page, int pageSize, CancellationToken ct = default);
}

public class BookingDapperRepository : IBookingReadRepository
{
    private readonly string _connectionString;

    public BookingDapperRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("BookingDb")
           ?? throw new InvalidOperationException("BookingDb connection string not configured.");

    public async Task<BookingRequestDto?> GetBookingDetailsAsync(long bookingNumber, CancellationToken ct = default)
    {
        const string sql = """
            SELECT 
                CAST(br.BK_BOK_NUM AS BIGINT) AS BookingNumber,
                br.BK_USR_COD   AS UserCode,
                CAST(br.BK_USR_NUM AS BIGINT) AS UserNum,
                br.BK_BOK_TYP   AS BookingType,
                br.BK_FRO_DAT   AS DepartureDate,
                br.BK_RET_DAT   AS ReturnDate,
                CAST(br.BK_FRO_CIT AS BIGINT) AS FromCity,
                CAST(br.BK_TO_CIT  AS BIGINT) AS ToCity,
                br.BK_FRO_LOC   AS FromLocation,
                br.BK_TO_LOC    AS ToLocation,
                br.BK_PER_NAM   AS PersonName,
                CAST(ISNULL(br.BK_BUD_AMT,0) AS DECIMAL(19,2)) AS BudgetAmount,
                br.BK_APP_STS   AS Status,
                CAST(br.BK_CNF_NUM AS BIGINT) AS ConfirmationNumber,
                br.BK_CAN_DAT   AS CancelledOn,
                br.BK_CAN_REM   AS CancellationRemarks
            FROM dbo.BOOK_REQUEST br
            WHERE br.BK_BOK_NUM = @BookingNumber
            """;

        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync<BookingRequestDto>(
            new CommandDefinition(sql, new { BookingNumber = bookingNumber }, cancellationToken: ct));
    }

    public async Task<BookingConfirmationDto?> GetConfirmationDetailsAsync(long confirmationNumber, CancellationToken ct = default)
    {
        const string sql = """
            SELECT 
                bc.BK_CNF_NUM   AS ConfirmationNumber,
                bc.BK_BOK_NUM   AS BookingNumber,
                bc.BK_MOD_COD   AS ModeOfTravel,
                bc.BK_FRO_CIT   AS FromCity,
                bc.BK_TO_CIT    AS ToCity,
                bc.BK_FRO_DAT   AS DepartureDate,
                bc.BK_TO_DAT    AS ReturnDate,
                bc.BK_VND_COD   AS VendorCode,
                bc.BK_TCK_NUM   AS TicketNumber,
                bc.BK_ADM_RMK   AS AdminRemarks,
                bc.BK_STS_COD   AS Status
            FROM dbo.BOOK_CONFIRMATION bc
            WHERE bc.BK_CNF_NUM = @ConfirmationNumber
            """;

        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync<BookingConfirmationDto>(
            new CommandDefinition(sql, new { ConfirmationNumber = confirmationNumber }, cancellationToken: ct));
    }

    public async Task<IEnumerable<BookingListDto>> GetUserBookingsAsync(string userCode, int page, int pageSize, CancellationToken ct = default)
    {
        const string sql = """
            SELECT 
                CAST(br.BK_BOK_NUM AS BIGINT) AS BookingNumber,
                br.BK_USR_COD   AS UserCode,
                br.BK_BOK_TYP   AS BookingType,
                br.BK_FRO_DAT   AS DepartureDate,
                br.BK_RET_DAT   AS ReturnDate,
                br.BK_APP_STS   AS Status,
                br.BK_PER_NAM   AS PersonName
            FROM dbo.BOOK_REQUEST br
            WHERE br.BK_USR_COD = @UserCode
            ORDER BY br.BK_APP_DAT DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<BookingListDto>(
            new CommandDefinition(sql,
                new { UserCode = userCode, Offset = (page - 1) * pageSize, PageSize = pageSize },
                cancellationToken: ct));
    }
}
