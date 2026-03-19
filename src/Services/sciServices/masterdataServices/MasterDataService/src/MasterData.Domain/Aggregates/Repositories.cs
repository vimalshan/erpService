using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MasterData.Domain.Entities;

#nullable enable

namespace MasterData.Domain.Aggregates
{
    /// <summary>
    /// Repository interface for CompanyUnit aggregate
    /// </summary>
    public interface ICompanyUnitRepository
    {
        Task<Entities.CompanyUnitAggregate?> GetByIdAsync(int id);
        Task<Entities.CompanyUnitAggregate?> GetByCodeAsync(string code);
        Task<IReadOnlyList<Entities.CompanyUnitAggregate>> GetAllAsync();
        Task AddAsync(Entities.CompanyUnitAggregate companyUnit);
        Task UpdateAsync(Entities.CompanyUnitAggregate companyUnit);
        Task DeleteAsync(int id);
    }

    /// <summary>
    /// Repository interface for Location aggregate
    /// </summary>
    public interface ILocationRepository
    {
        Task<Entities.LocationAggregate?> GetByIdAsync(int id);
        Task<IReadOnlyList<Entities.LocationAggregate>> GetAllAsync();
        Task AddAsync(Entities.LocationAggregate location);
        Task UpdateAsync(Entities.LocationAggregate location);
        Task DeleteAsync(int id);
    }

    /// <summary>
    /// Repository interface for Supplier aggregate
    /// </summary>
    public interface ISupplierRepository
    {
        Task<Entities.SupplierAggregate?> GetByIdAsync(string id);
        Task<Entities.SupplierAggregate?> GetByCodeAsync(string code);
        Task<IReadOnlyList<Entities.SupplierAggregate>> GetAllAsync();
        Task AddAsync(Entities.SupplierAggregate supplier);
        Task UpdateAsync(Entities.SupplierAggregate supplier);
        Task DeleteAsync(string id);
    }

    /// <summary>
    /// Repository interface for State aggregate
    /// </summary>
    public interface IStateRepository
    {
        Task<Entities.StateAggregate?> GetByIdAsync(string id);
        Task<Entities.StateAggregate?> GetByCodeAsync(string code);
        Task<IReadOnlyList<Entities.StateAggregate>> GetAllAsync();
        Task AddAsync(Entities.StateAggregate state);
        Task UpdateAsync(Entities.StateAggregate state);
        Task DeleteAsync(string id);
    }

    /// <summary>
    /// Repository interface for City aggregate
    /// </summary>
    public interface ICityRepository
    {
        Task<Entities.CityAggregate?> GetByIdAsync(string id);
        Task<Entities.CityAggregate?> GetByCodeAsync(string code);
        Task<IReadOnlyList<Entities.CityAggregate>> GetAllAsync();
        Task<IReadOnlyList<Entities.CityAggregate>> GetByStateCodeAsync(string stateCode);
        Task AddAsync(Entities.CityAggregate city);
        Task UpdateAsync(Entities.CityAggregate city);
        Task DeleteAsync(string id);
    }

    /// <summary>
    /// Unit of Work interface for coordinating multiple repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        ICompanyUnitRepository CompanyUnits { get; }
        ILocationRepository Locations { get; }
        ISupplierRepository Suppliers { get; }
        IStateRepository States { get; }
        ICityRepository Cities { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
