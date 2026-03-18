using CashManagement.Domain.Common;
using CashManagement.Domain.Events;
using CashManagement.Domain.Exceptions;
using CashManagement.Domain.ValueObjects;

namespace CashManagement.Domain.Entities;

public class ChequeRegister : AggregateRoot
{
    public long BankAccountId { get; private set; }
    public string ChequeNumber { get; private set; } = default!;
    public string PayeeName { get; private set; } = default!;
    public decimal ChequeAmount { get; private set; }
    public DateOnly IssueDate { get; private set; }
    public DateOnly ChequeDate { get; private set; }
    public string? Reference { get; private set; }
    public ChequeStatus Status { get; private set; }
    public string? BounceReason { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private ChequeRegister() { }

    public static ChequeRegister Issue(long bankAccountId, string chequeNumber, string payeeName,
        decimal amount, DateOnly chequeDate, string? reference, long issuedBy)
    {
        if (amount <= 0) throw new DomainException("Cheque amount must be greater than zero.");
        var cheque = new ChequeRegister
        {
            BankAccountId = bankAccountId,
            ChequeNumber = chequeNumber,
            PayeeName = payeeName,
            ChequeAmount = amount,
            IssueDate = DateOnly.FromDateTime(DateTime.UtcNow),
            ChequeDate = chequeDate,
            Reference = reference,
            Status = ChequeStatus.Issued,
            CreatedBy = issuedBy,
            CreatedOn = DateTime.UtcNow
        };
        cheque.AddDomainEvent(new ChequeIssuedEvent(cheque.BankAccountId, cheque.ChequeNumber, cheque.PayeeName, cheque.ChequeAmount));
        return cheque;
    }

    public void MarkCleared(long updatedBy)
    {
        if (Status != ChequeStatus.Issued)
            throw new ChequeStatusTransitionException(Status.ToString(), ChequeStatus.Cleared.ToString());
        Status = ChequeStatus.Cleared;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void MarkBounced(string reason, long updatedBy)
    {
        if (Status != ChequeStatus.Issued)
            throw new ChequeStatusTransitionException(Status.ToString(), ChequeStatus.Bounced.ToString());
        Status = ChequeStatus.Bounced;
        BounceReason = reason;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new ChequeBouncedEvent(BankAccountId, ChequeNumber, reason, ChequeAmount));
    }

    public void Cancel(long updatedBy)
    {
        if (Status == ChequeStatus.Cleared)
            throw new ChequeStatusTransitionException(Status.ToString(), ChequeStatus.Cancelled.ToString());
        Status = ChequeStatus.Cancelled;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
