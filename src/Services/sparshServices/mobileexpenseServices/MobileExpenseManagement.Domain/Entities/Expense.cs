using MediatR;

namespace MobileExpenseManagement.Domain.Entities;

/// <summary>
/// Represents a mobile expense entity
/// </summary>
public class Expense
{
    public decimal Id { get; private set; }
    public decimal TripId { get; private set; }
    public decimal CategoryId { get; private set; }
    public DateTime ExpenseDate { get; private set; }
    public string Comment { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public decimal? CurrencyId { get; private set; }
    public decimal EnteredBy { get; private set; }
    public DateTime EnteredOn { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public decimal? ModifiedBy { get; private set; }
    public DateTime? DeletedOn { get; private set; }
    public decimal? DeletedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation property
    public ICollection<ExpenseFile> Files { get; set; } = new List<ExpenseFile>();

    // Domain events
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Create a new expense
    /// </summary>
    public static Expense Create(decimal tripId, decimal categoryId, DateTime expenseDate, string comment, 
        decimal amount, decimal? currencyId, decimal enteredBy)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0", nameof(amount));

        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment is required", nameof(comment));

        if (expenseDate > DateTime.UtcNow)
            throw new ArgumentException("Expense date cannot be in the future", nameof(expenseDate));

        var expense = new Expense
        {
            Id = 0, // Will be assigned by database
            TripId = tripId,
            CategoryId = categoryId,
            ExpenseDate = expenseDate,
            Comment = comment.Trim(),
            Amount = amount,
            CurrencyId = currencyId,
            EnteredBy = enteredBy,
            EnteredOn = DateTime.UtcNow,
            IsDeleted = false
        };

        expense.AddDomainEvent(new ExpenseCreatedDomainEvent(expense.Id, tripId, categoryId, amount));
        return expense;
    }

    /// <summary>
    /// Update expense details
    /// </summary>
    public void Update(string comment, decimal amount, decimal? currencyId, decimal modifiedBy)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0", nameof(amount));

        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment is required", nameof(comment));

        Comment = comment.Trim();
        Amount = amount;
        CurrencyId = currencyId;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new ExpenseUpdatedDomainEvent(Id, Amount));
    }

    /// <summary>
    /// Soft delete the expense
    /// </summary>
    public void Delete(decimal deletedBy)
    {
        IsDeleted = true;
        DeletedBy = deletedBy;
        DeletedOn = DateTime.UtcNow;

        AddDomainEvent(new ExpenseDeletedDomainEvent(Id));
    }

    /// <summary>
    /// Add a file to the expense
    /// </summary>
    public void AddFile(ExpenseFile file)
    {
        if (file == null)
            throw new ArgumentNullException(nameof(file));

        Files.Add(file);
    }

    /// <summary>
    /// Remove a file from the expense
    /// </summary>
    public void RemoveFile(ExpenseFile file)
    {
        if (file == null)
            throw new ArgumentNullException(nameof(file));

        Files.Remove(file);
    }

    // Domain event handling
    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

/// <summary>
/// Base class for domain events
/// </summary>
public abstract class DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <summary>
/// Event raised when expense is created
/// </summary>
public class ExpenseCreatedDomainEvent : DomainEvent
{
    public decimal ExpenseId { get; }
    public decimal TripId { get; }
    public decimal CategoryId { get; }
    public decimal Amount { get; }

    public ExpenseCreatedDomainEvent(decimal expenseId, decimal tripId, decimal categoryId, decimal amount)
    {
        ExpenseId = expenseId;
        TripId = tripId;
        CategoryId = categoryId;
        Amount = amount;
    }
}

/// <summary>
/// Event raised when expense is updated
/// </summary>
public class ExpenseUpdatedDomainEvent : DomainEvent
{
    public decimal ExpenseId { get; }
    public decimal NewAmount { get; }

    public ExpenseUpdatedDomainEvent(decimal expenseId, decimal newAmount)
    {
        ExpenseId = expenseId;
        NewAmount = newAmount;
    }
}

/// <summary>
/// Event raised when expense is deleted
/// </summary>
public class ExpenseDeletedDomainEvent : DomainEvent
{
    public decimal ExpenseId { get; }

    public ExpenseDeletedDomainEvent(decimal expenseId)
    {
        ExpenseId = expenseId;
    }
}
