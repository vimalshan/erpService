using StipendService.Domain.Common;
using StipendService.Domain.Events;
using StipendService.Domain.Exceptions;
using StipendService.Domain.ValueObjects;

namespace StipendService.Domain.Entities;

/// <summary>
/// Entity: SRF Stipend Disbursement - tracks an individual stipend payment.
/// </summary>
public sealed class StipendDisbursement : AuditableEntity
{
    public long SrfId { get; private set; }
    public long StipendId { get; private set; }
    public DateTime DisbursementDate { get; private set; }
    public decimal DisbursementAmount { get; private set; }
    public string DisbursementStatus { get; private set; } = "D";
    public string? MonthYear { get; private set; }
    public string? BankReference { get; private set; }
    public string? ReferenceNo { get; private set; }

    public StipendMaster? StipendMaster { get; private set; }

    private StipendDisbursement() { }

    public static StipendDisbursement Create(
        long srfId,
        long stipendId,
        DateTime disbursementDate,
        decimal amount,
        string monthYear,
        long createdBy)
    {
        if (srfId <= 0) throw new DomainException("SrfId must be positive.");
        if (stipendId <= 0) throw new DomainException("StipendId must be positive.");
        if (amount <= 0) throw new DomainException("Disbursement amount must be positive.");

        var disbursement = new StipendDisbursement
        {
            SrfId = srfId,
            StipendId = stipendId,
            DisbursementDate = disbursementDate,
            DisbursementAmount = amount,
            DisbursementStatus = "D",
            MonthYear = monthYear,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        disbursement.AddDomainEvent(new DisbursementCreatedEvent(disbursement));
        return disbursement;
    }

    public void Process(long updatedBy)
    {
        if (DisbursementStatus != "D")
            throw new DomainException($"Only Draft disbursements can be processed. Current status: {DisbursementStatus}");

        DisbursementStatus = "P";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new DisbursementProcessedEvent(this));
    }

    public void Reject(long updatedBy)
    {
        if (DisbursementStatus == "R")
            throw new DomainException("Disbursement is already rejected.");

        DisbursementStatus = "R";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new DisbursementRejectedEvent(this));
    }

    public void SetBankReference(string bankReference, string referenceNo, long updatedBy)
    {
        BankReference = bankReference;
        ReferenceNo = referenceNo;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
