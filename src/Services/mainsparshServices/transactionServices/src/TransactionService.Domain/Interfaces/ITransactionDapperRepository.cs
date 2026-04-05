namespace TransactionService.Domain.Interfaces;

public interface ITransactionDapperRepository
{
    Task<IEnumerable<dynamic>> GetPendingApprovalsAsync(long? approverId = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<dynamic>> GetAuditLogAsync(string? entityType = null, long? entityId = null, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<dynamic>> GetPendingDisbursementsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<dynamic>> GetAvailableRoomsAsync(DateTime date, TimeSpan startTime, TimeSpan endTime, CancellationToken cancellationToken = default);
    Task<bool> ValidateBookingAttendeesAsync(long bookingId, CancellationToken cancellationToken = default);
    Task<decimal> CalculateSRFStipendAsync(long researchCategoryId, long rankId, CancellationToken cancellationToken = default);
    Task<int> ProcessMonthlyStipendAsync(int month, int year, long processedBy, CancellationToken cancellationToken = default);
}
