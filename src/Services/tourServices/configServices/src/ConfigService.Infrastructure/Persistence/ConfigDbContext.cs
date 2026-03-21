using ConfigService.Domain.Common;
using ConfigService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConfigService.Infrastructure.Persistence;

public class ConfigDbContext(DbContextOptions<ConfigDbContext> options, IMediator mediator) : DbContext(options), IUnitOfWork
{
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<ExpenseCurrency> ExpenseCurrencies => Set<ExpenseCurrency>();
    public DbSet<ExpenseGroup> ExpenseGroups => Set<ExpenseGroup>();
    public DbSet<ExpenseGroupMap> ExpenseGroupMaps => Set<ExpenseGroupMap>();
    public DbSet<ExpenseType> ExpenseTypes => Set<ExpenseType>();
    public DbSet<GlobalPayParam> GlobalPayParams => Set<GlobalPayParam>();
    public DbSet<GradeCatExpenseRule> GradeCatExpenseRules => Set<GradeCatExpenseRule>();
    public DbSet<GradeCatExpenseRuleBreak> GradeCatExpenseRuleBreaks => Set<GradeCatExpenseRuleBreak>();
    public DbSet<GradeCatModeMap> GradeCatModeMaps => Set<GradeCatModeMap>();
    public DbSet<GradeCatStayRule> GradeCatStayRules => Set<GradeCatStayRule>();
    public DbSet<GradeCatExpenseMap> GradeCatExpenseMaps => Set<GradeCatExpenseMap>();
    public DbSet<GradeTypeTravelParam> GradeTypeTravelParams => Set<GradeTypeTravelParam>();
    public DbSet<CalendarGstBuMap> CalendarGstBuMaps => Set<CalendarGstBuMap>();
    public DbSet<TravelCity> TravelCities => Set<TravelCity>();
    public DbSet<TravelCityModeMap> TravelCityModeMaps => Set<TravelCityModeMap>();
    public DbSet<TravelCitySectorMap> TravelCitySectorMaps => Set<TravelCitySectorMap>();
    public DbSet<TravelClass> TravelClasses => Set<TravelClass>();
    public DbSet<TravelContact> TravelContacts => Set<TravelContact>();
    public DbSet<TravelCountry> TravelCountries => Set<TravelCountry>();
    public DbSet<TravelCountryModeMap> TravelCountryModeMaps => Set<TravelCountryModeMap>();
    public DbSet<TravelCountrySectorMap> TravelCountrySectorMaps => Set<TravelCountrySectorMap>();
    public DbSet<TravelCountryCurrencyMap> TravelCountryCurrencyMaps => Set<TravelCountryCurrencyMap>();
    public DbSet<TravelBusCitySectorMap> TravelBusCitySectorMaps => Set<TravelBusCitySectorMap>();
    public DbSet<TravelBuExclude> TravelBuExcludes => Set<TravelBuExclude>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<VendorTaxRate> VendorTaxRates => Set<VendorTaxRate>();
    public DbSet<VendorUnitMap> VendorUnitMaps => Set<VendorUnitMap>();
    public DbSet<VendorCharges> VendorCharges => Set<VendorCharges>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConfigDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity<string>>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEntitiesLong = ChangeTracker.Entries<BaseEntity<long>>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEntitiesInt = ChangeTracker.Entries<BaseEntity<int>>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.DomainEvents)
            .Concat(domainEntitiesLong.SelectMany(e => e.DomainEvents))
            .Concat(domainEntitiesInt.SelectMany(e => e.DomainEvents))
            .ToList();

        foreach (var entity in domainEntities) entity.ClearDomainEvents();
        foreach (var entity in domainEntitiesLong) entity.ClearDomainEvents();
        foreach (var entity in domainEntitiesInt) entity.ClearDomainEvents();

        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent, ct);

        return result;
    }
}
