using ExpenseService.Domain.Common;
using ExpenseService.Domain.Entities;

namespace ExpenseService.Domain.Aggregates;

public class ExpenseAggregate : IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public TravelExpense RootExpense { get; private set; }
    public IReadOnlyList<TravelExpenseAllocation> Allocations => RootExpense.Allocations.ToList().AsReadOnly();
    public IReadOnlyList<TravelExpenseSub> SubDetails => RootExpense.SubDetails.ToList().AsReadOnly();
    public DaSummary? DaSummary { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public ExpenseAggregate(TravelExpense rootExpense)
    {
        RootExpense = rootExpense ?? throw new ArgumentNullException(nameof(rootExpense));
    }

    public void RecordExpense(long expenseCode, decimal budgetAmount, decimal selfAmount, string? remarks)
    {
        RootExpense.ExpenseCode = expenseCode;
        RootExpense.BudgetAmount = budgetAmount;
        RootExpense.SelfExpense = selfAmount;
        RootExpense.ExpenseRemarks = remarks;
        RootExpense.VarianceAmount = (RootExpense.EligibleAmount ?? 0) - budgetAmount;

        _domainEvents.Add(new Events.ExpenseRecordedEvent(
            RootExpense.RequestNumber,
            RootExpense.SerialNumber,
            budgetAmount));
    }

    public void SettleExpense(decimal settlementAmount, decimal refundAmount)
    {
        _domainEvents.Add(new Events.ExpenseSettledEvent(
            RootExpense.RequestNumber,
            settlementAmount,
            refundAmount));
    }

    public void SetDaSummary(DaSummary summary)
    {
        DaSummary = summary;
        _domainEvents.Add(new Events.DACalculatedEvent(
            summary.RequestId,
            summary.AdminAmount + summary.SelfAmount));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
