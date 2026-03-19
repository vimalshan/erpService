using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MasterData.Domain.Aggregates;
using MasterData.Infrastructure.Persistence;
using MasterData.Infrastructure.Repositories;

#nullable enable

namespace MasterData.Infrastructure.Services
{
    /// <summary>
    /// Unit of Work implementation coordinating multiple repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MasterDataDbContext _context;
        private ICompanyUnitRepository? _companyUnitRepository;
        private ILocationRepository? _locationRepository;
        private ISupplierRepository? _supplierRepository;
        private IStateRepository? _stateRepository;
        private ICityRepository? _cityRepository;

        public UnitOfWork(MasterDataDbContext context)
        {
            _context = context;
        }

        public ICompanyUnitRepository CompanyUnits
        {
            get { return _companyUnitRepository ??= new CompanyUnitRepository(_context); }
        }

        public ILocationRepository Locations
        {
            get { return _locationRepository ??= new LocationRepository(_context); }
        }

        public ISupplierRepository Suppliers
        {
            get { return _supplierRepository ??= new SupplierRepository(_context); }
        }

        public IStateRepository States
        {
            get { return _stateRepository ??= new StateRepository(_context); }
        }

        public ICityRepository Cities
        {
            get { return _cityRepository ??= new CityRepository(_context); }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
            }
            catch
            {
                await _context.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
