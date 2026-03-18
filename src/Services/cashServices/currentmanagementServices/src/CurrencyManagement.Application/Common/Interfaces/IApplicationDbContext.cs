namespace CurrencyManagement.Application.Common.Interfaces;

using CurrencyManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Application database context interface
/// </summary>
public interface IApplicationDbContext
{
    DbSet<Currency> Currencies { get; }
    DbSet<ExchangeRate> ExchangeRates { get; }
    DbSet<OrganizationCurrencyMapping> OrganizationCurrencyMappings { get; }

    /// <summary>
    /// Saves all changes to the database
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
