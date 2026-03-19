using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MasterData.Domain.Aggregates;
using MasterData.Domain.Entities;
using MasterData.Infrastructure.Persistence;

#nullable enable

namespace MasterData.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for CompanyUnit aggregate
    /// </summary>
    public class CompanyUnitRepository : ICompanyUnitRepository
    {
        private readonly MasterDataDbContext _context;

        public CompanyUnitRepository(MasterDataDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyUnitAggregate?> GetByIdAsync(int id)
        {
            return await _context.CompanyUnits.FindAsync(id);
        }

        public async Task<CompanyUnitAggregate?> GetByCodeAsync(string code)
        {
            return _context.CompanyUnits.FirstOrDefault(x => x.Code == code && !x.IsDeleted);
        }

        public async Task<IReadOnlyList<CompanyUnitAggregate>> GetAllAsync()
        {
            return _context.CompanyUnits.Where(x => !x.IsDeleted).ToList().AsReadOnly();
        }

        public async Task AddAsync(CompanyUnitAggregate companyUnit)
        {
            _context.CompanyUnits.Add(companyUnit);
        }

        public async Task UpdateAsync(CompanyUnitAggregate companyUnit)
        {
            _context.CompanyUnits.Update(companyUnit);
        }

        public async Task DeleteAsync(int id)
        {
            var unit = await GetByIdAsync(id);
            if (unit != null)
            {
                _context.CompanyUnits.Remove(unit);
            }
        }
    }

    /// <summary>
    /// Repository implementation for Location aggregate
    /// </summary>
    public class LocationRepository : ILocationRepository
    {
        private readonly MasterDataDbContext _context;

        public LocationRepository(MasterDataDbContext context)
        {
            _context = context;
        }

        public async Task<LocationAggregate?> GetByIdAsync(int id)
        {
            return await _context.Locations.FindAsync(id);
        }

        public async Task<IReadOnlyList<LocationAggregate>> GetAllAsync()
        {
            return _context.Locations.Where(x => !x.IsDeleted).ToList().AsReadOnly();
        }

        public async Task AddAsync(LocationAggregate location)
        {
            _context.Locations.Add(location);
        }

        public async Task UpdateAsync(LocationAggregate location)
        {
            _context.Locations.Update(location);
        }

        public async Task DeleteAsync(int id)
        {
            var location = await GetByIdAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
            }
        }
    }

    /// <summary>
    /// Repository implementation for Supplier aggregate
    /// </summary>
    public class SupplierRepository : ISupplierRepository
    {
        private readonly MasterDataDbContext _context;

        public SupplierRepository(MasterDataDbContext context)
        {
            _context = context;
        }

        public async Task<SupplierAggregate?> GetByIdAsync(string id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<SupplierAggregate?> GetByCodeAsync(string code)
        {
            return _context.Suppliers.FirstOrDefault(x => x.Code == code && !x.IsDeleted);
        }

        public async Task<IReadOnlyList<SupplierAggregate>> GetAllAsync()
        {
            return _context.Suppliers.Where(x => !x.IsDeleted).ToList().AsReadOnly();
        }

        public async Task AddAsync(SupplierAggregate supplier)
        {
            _context.Suppliers.Add(supplier);
        }

        public async Task UpdateAsync(SupplierAggregate supplier)
        {
            _context.Suppliers.Update(supplier);
        }

        public async Task DeleteAsync(string id)
        {
            var supplier = await GetByIdAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
            }
        }
    }

    /// <summary>
    /// Repository implementation for State aggregate
    /// </summary>
    public class StateRepository : IStateRepository
    {
        private readonly MasterDataDbContext _context;

        public StateRepository(MasterDataDbContext context)
        {
            _context = context;
        }

        public async Task<StateAggregate?> GetByIdAsync(string id)
        {
            return await _context.States.FindAsync(id);
        }

        public async Task<StateAggregate?> GetByCodeAsync(string code)
        {
            return _context.States.FirstOrDefault(x => x.Code == code && !x.IsDeleted);
        }

        public async Task<IReadOnlyList<StateAggregate>> GetAllAsync()
        {
            return _context.States.Where(x => !x.IsDeleted).ToList().AsReadOnly();
        }

        public async Task AddAsync(StateAggregate state)
        {
            _context.States.Add(state);
        }

        public async Task UpdateAsync(StateAggregate state)
        {
            _context.States.Update(state);
        }

        public async Task DeleteAsync(string id)
        {
            var state = await GetByIdAsync(id);
            if (state != null)
            {
                _context.States.Remove(state);
            }
        }
    }

    /// <summary>
    /// Repository implementation for City aggregate
    /// </summary>
    public class CityRepository : ICityRepository
    {
        private readonly MasterDataDbContext _context;

        public CityRepository(MasterDataDbContext context)
        {
            _context = context;
        }

        public async Task<CityAggregate?> GetByIdAsync(string id)
        {
            return await _context.Cities.FindAsync(id);
        }

        public async Task<CityAggregate?> GetByCodeAsync(string code)
        {
            return _context.Cities.FirstOrDefault(x => x.Code == code && !x.IsDeleted);
        }

        public async Task<IReadOnlyList<CityAggregate>> GetAllAsync()
        {
            return _context.Cities.Where(x => !x.IsDeleted).ToList().AsReadOnly();
        }

        public async Task<IReadOnlyList<CityAggregate>> GetByStateCodeAsync(string stateCode)
        {
            return _context.Cities.Where(x => x.StateCode == stateCode && !x.IsDeleted).ToList().AsReadOnly();
        }

        public async Task AddAsync(CityAggregate city)
        {
            _context.Cities.Add(city);
        }

        public async Task UpdateAsync(CityAggregate city)
        {
            _context.Cities.Update(city);
        }

        public async Task DeleteAsync(string id)
        {
            var city = await GetByIdAsync(id);
            if (city != null)
            {
                _context.Cities.Remove(city);
            }
        }
    }
}
