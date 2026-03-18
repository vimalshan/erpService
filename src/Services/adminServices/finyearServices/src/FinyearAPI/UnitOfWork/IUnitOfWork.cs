using FinyearAPI.Models;
using FinyearAPI.Repositories.Interfaces;
using FinyearAPI.Repositories.Dapper;

namespace FinyearAPI.UnitOfWork
{
    /// <summary>
    /// Unit of Work Interface
    /// Coordinates work across multiple repositories and manages transactions
    /// </summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        IFinancialYearRepository FinancialYearRepository { get; }
        IFinancialYearDapperRepository FinancialYearDapperRepository { get; }

        Task<int> SaveChangesAsync();
        Task<bool> BeginTransactionAsync();
        Task<bool> CommitAsync();
        Task<bool> RollbackAsync();
    }
}
