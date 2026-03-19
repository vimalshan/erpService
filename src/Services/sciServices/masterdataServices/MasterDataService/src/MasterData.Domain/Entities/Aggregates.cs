using System;
using System.Collections.Generic;
using System.Linq;
using MasterData.Domain.Events;

#nullable enable

namespace MasterData.Domain.Entities
{
    /// <summary>
    /// Base entity class for all domain entities
    /// </summary>
    public abstract class Entity<TId> 
    {
        public TId Id { get; protected set; } = default!;
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public bool IsDeleted { get; protected set; }

        protected Entity() { }

        protected Entity(TId id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TId> other)
                return false;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    /// <summary>
    /// Aggregate Root for Company Unit
    /// </summary>
    public class CompanyUnitAggregate : Entity<int>
    {
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        private readonly List<DomainEvent> _domainEvents = [];

        public CompanyUnitAggregate() { }

        public CompanyUnitAggregate(int id, string code, string name) : base(id)
        {
            Code = code;
            Name = name;
        }

        public static CompanyUnitAggregate Create(string code, string name)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Company Unit Code cannot be empty", nameof(code));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Company Unit Name cannot be empty", nameof(name));

            var unit = new CompanyUnitAggregate
            {
                Code = code,
                Name = name,
                CreatedAt = DateTime.UtcNow
            };

            unit.AddDomainEvent(new CompanyUnitCreatedEvent(code, name));
            return unit;
        }

        public void Update(string code, string name)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Company Unit Code cannot be empty", nameof(code));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Company Unit Name cannot be empty", nameof(name));

            Code = code;
            Name = name;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new CompanyUnitUpdatedEvent(Id, code, name));
        }

        public void Delete()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new CompanyUnitDeletedEvent(Id));
        }

        public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Aggregate Root for Location
    /// </summary>
    public class LocationAggregate : Entity<int>
    {
        public string Name { get; private set; } = string.Empty;
        private readonly List<DomainEvent> _domainEvents = [];

        public LocationAggregate() { }

        public LocationAggregate(int id, string name) : base(id)
        {
            Name = name;
        }

        public static LocationAggregate Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Location Name cannot be empty", nameof(name));

            var location = new LocationAggregate
            {
                Name = name,
                CreatedAt = DateTime.UtcNow
            };

            location.AddDomainEvent(new LocationCreatedEvent(name));
            return location;
        }

        public void Update(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Location Name cannot be empty", nameof(name));

            Name = name;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new LocationUpdatedEvent(Id, name));
        }

        public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Aggregate Root for Supplier Master
    /// </summary>
    public class SupplierAggregate : Entity<string>
    {
        public string Code => Id;
        public string Name { get; private set; } = string.Empty;
        public string? Details { get; private set; }
        public string EntryId { get; private set; } = string.Empty;
        public decimal EntryNumber { get; private set; }
        private readonly List<DomainEvent> _domainEvents = [];

        public SupplierAggregate() { }

        public SupplierAggregate(string code, string name, string? details, string entryId, decimal entryNumber)
        {
            Id = code;
            Name = name;
            Details = details;
            EntryId = entryId;
            EntryNumber = entryNumber;
        }

        public static SupplierAggregate Create(string code, string name, string? details, string entryId, decimal entryNumber)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Supplier Code cannot be empty", nameof(code));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Supplier Name cannot be empty", nameof(name));

            if (string.IsNullOrWhiteSpace(entryId))
                throw new ArgumentException("Entry ID cannot be empty", nameof(entryId));

            var supplier = new SupplierAggregate(code, name, details, entryId, entryNumber)
            {
                CreatedAt = DateTime.UtcNow
            };

            supplier.AddDomainEvent(new SupplierCreatedEvent(code, name, details));
            return supplier;
        }

        public void Update(string name, string? details)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Supplier Name cannot be empty", nameof(name));

            Name = name;
            Details = details;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new SupplierUpdatedEvent(Code, name, details));
        }

        public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Aggregate Root for State Master
    /// </summary>
    public class StateAggregate : Entity<string>
    {
        public string Code => Id;
        public string Name { get; private set; } = string.Empty;
        private readonly List<DomainEvent> _domainEvents = [];

        public StateAggregate() { }

        public StateAggregate(string code, string name)
        {
            Id = code;
            Name = name;
        }

        public static StateAggregate Create(string code, string name)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("State Code cannot be empty", nameof(code));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("State Name cannot be empty", nameof(name));

            var state = new StateAggregate(code, name)
            {
                CreatedAt = DateTime.UtcNow
            };

            state.AddDomainEvent(new StateCreatedEvent(code, name));
            return state;
        }

        public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Aggregate Root for City Master
    /// </summary>
    public class CityAggregate : Entity<string>
    {
        public string Code => Id;
        public string Name { get; private set; } = string.Empty;
        public string StateCode { get; private set; } = string.Empty;
        private readonly List<DomainEvent> _domainEvents = [];

        public CityAggregate() { }

        public CityAggregate(string code, string name, string stateCode)
        {
            Id = code;
            Name = name;
            StateCode = stateCode;
        }

        public static CityAggregate Create(string code, string name, string stateCode)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("City Code cannot be empty", nameof(code));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("City Name cannot be empty", nameof(name));

            if (string.IsNullOrWhiteSpace(stateCode))
                throw new ArgumentException("State Code cannot be empty", nameof(stateCode));

            var city = new CityAggregate(code, name, stateCode)
            {
                CreatedAt = DateTime.UtcNow
            };

            city.AddDomainEvent(new CityCreatedEvent(code, name, stateCode));
            return city;
        }

        public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    }
}
