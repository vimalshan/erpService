using EmployeeService.Domain.Common;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.Repositories;
using EmployeeService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.Infrastructure.Repositories
{
    /// <summary>
    /// Unit of Work Pattern - Coordinates repository operations and transactions
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployeeServiceDbContext _context;
        private readonly IMediator _mediator;
        private IEmployeeRepository _employeeRepository;
        private IRepository<EmployeeAppraisal> _appraisalRepository;
        private IRepository<EmployeeCareerPlan> _careerPlanRepository;
        private IRepository<EmployeeBenefit> _benefitRepository;
        private IRepository<EmployeeAccountability> _accountabilityRepository;
        private IDbContextTransaction _transaction;

        public UnitOfWork(EmployeeServiceDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public IEmployeeRepository Employees
        {
            get
            {
                if (_employeeRepository == null)
                    _employeeRepository = new EmployeeRepository(_context);
                return _employeeRepository;
            }
        }

        public IRepository<EmployeeAppraisal> Appraisals
        {
            get
            {
                if (_appraisalRepository == null)
                    _appraisalRepository = new AppraisalRepository(_context);
                return _appraisalRepository;
            }
        }

        public IRepository<EmployeeCareerPlan> CareerPlans
        {
            get
            {
                if (_careerPlanRepository == null)
                    _careerPlanRepository = new CareerPlanRepository(_context);
                return _careerPlanRepository;
            }
        }

        public IRepository<EmployeeBenefit> Benefits
        {
            get
            {
                if (_benefitRepository == null)
                    _benefitRepository = new BenefitRepository(_context);
                return _benefitRepository;
            }
        }

        public IRepository<EmployeeAccountability> Accountabilities
        {
            get
            {
                if (_accountabilityRepository == null)
                    _accountabilityRepository = new AccountabilityRepository(_context);
                return _accountabilityRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                var domainEvents = _context.ChangeTracker
                    .Entries<BaseEntity>()
                    .Select(entry => entry.Entity)
                    .Where(entity => entity.DomainEvents.Any())
                    .SelectMany(entity =>
                    {
                        var events = entity.DomainEvents.ToList();
                        entity.ClearDomainEvents();
                        return events;
                    })
                    .ToList();

                var result = await _context.SaveChangesAsync();

                foreach (var domainEvent in domainEvents)
                {
                    await _mediator.Publish(domainEvent);
                }

                return result;
            }
            catch (Exception ex)
            {
                // Log exception here
                throw;
            }
        }

        public async Task<bool> BeginTransactionAsync()
        {
            try
            {
                _transaction = await _context.Database.BeginTransactionAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                if (_transaction != null)
                    await _transaction.CommitAsync();
                return true;
            }
            catch
            {
                await RollbackTransactionAsync();
                return false;
            }
        }

        public async Task<bool> RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                    await _transaction.RollbackAsync();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }

            if (_context != null)
            {
                await _context.DisposeAsync();
            }

            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Generic Repository wrapper for Appraisals
    /// </summary>
    internal class AppraisalRepository : GenericRepository<EmployeeAppraisal>
    {
        public AppraisalRepository(DbContext context) : base(context) { }
    }

    /// <summary>
    /// Generic Repository wrapper for Career Plans
    /// </summary>
    internal class CareerPlanRepository : GenericRepository<EmployeeCareerPlan>
    {
        public CareerPlanRepository(DbContext context) : base(context) { }
    }

    /// <summary>
    /// Generic Repository wrapper for Benefits
    /// </summary>
    internal class BenefitRepository : GenericRepository<EmployeeBenefit>
    {
        public BenefitRepository(DbContext context) : base(context) { }
    }

    /// <summary>
    /// Generic Repository wrapper for Accountabilities
    /// </summary>
    internal class AccountabilityRepository : GenericRepository<EmployeeAccountability>
    {
        public AccountabilityRepository(DbContext context) : base(context) { }
    }
}

