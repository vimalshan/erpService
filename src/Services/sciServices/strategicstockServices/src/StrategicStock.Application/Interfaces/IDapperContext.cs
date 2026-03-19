namespace StrategicStock.Application.Interfaces;

public interface IDapperContext
{
    Task<IReadOnlyList<T>> QueryStoredProcAsync<T>(string storedProcedure, object? parameters = null);
    Task<T?> QuerySingleStoredProcAsync<T>(string storedProcedure, object? parameters = null);
}
