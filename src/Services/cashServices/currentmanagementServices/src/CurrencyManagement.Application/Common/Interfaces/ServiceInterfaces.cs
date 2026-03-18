namespace CurrencyManagement.Application.Common.Interfaces;

/// <summary>
/// Interface for publishing domain events
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes a message to a message broker (e.g., RabbitMQ)
    /// </summary>
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
}

/// <summary>
/// Interface for currency (read) query service
/// </summary>
public interface ICurrencyQueryService
{
    /// <summary>
    /// Gets a currency by ID (read-optimized via Dapper)
    /// </summary>
    Task<dynamic?> GetCurrencyByIdAsync(long currencyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all currencies (read-optimized via Dapper)
    /// </summary>
    Task<IList<dynamic>> GetAllCurrenciesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a currency by name
    /// </summary>
    Task<dynamic?> GetCurrencyByNameAsync(string name, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for exchange rate query service
/// </summary>
public interface IExchangeRateQueryService
{
    /// <summary>
    /// Gets the latest exchange rate for a currency pair
    /// </summary>
    Task<dynamic?> GetLatestRateAsync(long fromCurrencyId, long toCurrencyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an exchange rate for a specific period
    /// </summary>
    Task<dynamic?> GetRateByPeriodAsync(long fromCurrencyId, long toCurrencyId, long financialYear, long month, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all exchange rates for a specific period
    /// </summary>
    Task<IList<dynamic>> GetRatesByPeriodAsync(long financialYear, long month, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for blob storage operations
/// </summary>
public interface IBlobStorageService
{
    /// <summary>
    /// Uploads a file to blob storage
    /// </summary>
    Task<string> UploadAsync(string containerName, string fileName, Stream content, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file from blob storage
    /// </summary>
    Task DeleteAsync(string containerName, string fileName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets download URL for a blob
    /// </summary>
    Task<string> GetDownloadUrlAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
}
