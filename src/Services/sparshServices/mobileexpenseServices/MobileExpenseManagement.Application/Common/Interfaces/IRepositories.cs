namespace MobileExpenseManagement.Application.Common.Interfaces;

using MobileExpenseManagement.Domain.Entities;

/// <summary>
/// Repository interface for Expense entity
/// </summary>
public interface IExpenseRepository
{
    Task AddAsync(Expense expense, CancellationToken cancellationToken = default);
    Task<Expense?> GetByIdAsync(decimal expenseId, CancellationToken cancellationToken = default);
    Task<List<Expense>> GetByTripIdAsync(decimal tripId, CancellationToken cancellationToken = default);
    Task<(List<Expense>, int)> GetByTripIdPaginatedAsync(decimal tripId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<List<ExpenseFile>> GetExpenseFilesAsync(decimal expenseId, CancellationToken cancellationToken = default);
    Task<List<Expense>> SearchByDateRangeAsync(DateTime startDate, DateTime endDate, decimal? tripId, decimal? categoryId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Expense expense, CancellationToken cancellationToken = default);
    Task DeleteAsync(decimal expenseId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Unit of Work interface
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IExpenseRepository Expenses { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<bool> CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task<bool> RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Blob storage service interface
/// </summary>
public interface IBlobStorageService
{
    Task<string> UploadFileAsync(string containerName, string blobName, byte[] fileContent, string contentType, CancellationToken cancellationToken = default);
    Task<byte[]> DownloadFileAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<bool> DeleteFileAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<Uri> GetFileUriAsync(string containerName, string blobName);
}

/// <summary>
/// Message bus interface for RabbitMQ
/// </summary>
public interface IMessageBus
{
    Task PublishAsync<T>(T message, string? routingKey = null, CancellationToken cancellationToken = default) where T : class;
    Task SubscribeAsync<T>(string queueName, Func<T, Task> handler) where T : class;
}

/// <summary>
/// Email service interface
/// </summary>
public interface IEmailService
{
    Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    Task SendAsync(List<string> recipients, string subject, string body, CancellationToken cancellationToken = default);
}
