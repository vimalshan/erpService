using BookingService.Application.Common;
using BookingService.Application.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BookingService.Infrastructure.Dapper;

public interface IBookingQueryRepository
{
    Task<IEnumerable<BookingDto>> SearchBookingsAsync(string? keyword, string? status, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<AttendeeDto>> GetAttendeesByBookingIdAsync(long bookingId, CancellationToken cancellationToken = default);
    Task<BookingService.Application.Common.PagedResponse<BookingDto>> GetAllBookingsAsync(int page, int pageSize, string? statusFilter, CancellationToken cancellationToken = default);
}

public class BookingQueryRepository(IConfiguration configuration) : IBookingQueryRepository
{
    private SqlConnection CreateConnection()
        => new(configuration.GetConnectionString("BookingDb"));

    public async Task<IEnumerable<BookingDto>> SearchBookingsAsync(
        string? keyword, string? status, DateTime? fromDate, DateTime? toDate,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                BOOKING_ID         AS BookingId,
                BOOKING_APPNO      AS BookingAppNo,
                BOOKING_TITLE      AS BookingTitle,
                LOCATION_CODE      AS LocationCode,
                BOOKING_DATE       AS BookingDate,
                BOOKING_STATUS     AS Status,
                CREATED_BY         AS CreatedBy,
                CREATED_ON         AS CreatedOn,
                UPDATED_BY         AS UpdatedBy,
                UPDATED_ON         AS UpdatedOn
            FROM BOOK_MAIN
            WHERE (@Keyword IS NULL OR BOOKING_APPNO LIKE '%' + @Keyword + '%' OR BOOKING_TITLE LIKE '%' + @Keyword + '%')
              AND (@Status  IS NULL OR BOOKING_STATUS = @Status)
              AND (@FromDate IS NULL OR BOOKING_DATE >= @FromDate)
              AND (@ToDate   IS NULL OR BOOKING_DATE <= @ToDate)
            ORDER BY CREATED_ON DESC
            """;

        await using var conn = CreateConnection();
        return await conn.QueryAsync<BookingDto>(
            sql,
            new { Keyword = keyword, Status = status, FromDate = fromDate, ToDate = toDate });
    }

    public async Task<IEnumerable<AttendeeDto>> GetAttendeesByBookingIdAsync(
        long bookingId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                ATTENDEE_ID        AS AttendeeId,
                BOOKING_ID         AS BookingId,
                ATTENDEE_SYSID     AS AttendeeSysId,
                ATTENDEE_SERIAL    AS AttendeeSerial,
                ATTENDANCE_STATUS  AS AttendanceStatus,
                CREATED_BY         AS CreatedBy,
                CREATED_ON         AS CreatedOn
            FROM BOOK_ATTENDEES
            WHERE BOOKING_ID = @BookingId
            ORDER BY ATTENDEE_SERIAL
            """;

        await using var conn = CreateConnection();
        return await conn.QueryAsync<AttendeeDto>(sql, new { BookingId = bookingId });
    }

    public async Task<PagedResponse<BookingDto>> GetAllBookingsAsync(int page, int pageSize, string? statusFilter, CancellationToken cancellationToken = default)
    {
        const string countSql = """
            SELECT COUNT(*) 
            FROM BOOK_MAIN
            WHERE (@Status IS NULL OR BOOKING_STATUS = @Status)
            """;

        const string dataSql = """
            SELECT
                BOOKING_ID         AS BookingId,
                BOOKING_APPNO      AS BookingAppNo,
                BOOKING_TITLE      AS BookingTitle,
                LOCATION_CODE      AS LocationCode,
                BOOKING_DATE       AS BookingDate,
                BOOKING_STATUS     AS Status,
                CREATED_BY         AS CreatedBy,
                CREATED_ON         AS CreatedOn,
                UPDATED_BY         AS UpdatedBy,
                UPDATED_ON         AS UpdatedOn
            FROM BOOK_MAIN
            WHERE (@Status IS NULL OR BOOKING_STATUS = @Status)
            ORDER BY CREATED_ON DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        await using var conn = CreateConnection();
        
        var totalCount = await conn.ExecuteScalarAsync<int>(countSql, new { Status = statusFilter });
        var items = await conn.QueryAsync<BookingDto>(
            dataSql, 
            new { Status = statusFilter, Offset = (page - 1) * pageSize, PageSize = pageSize });

        return PagedResponse<BookingDto>.Create(items.ToList(), page, pageSize, totalCount);
    }
}
