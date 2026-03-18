using DeductionService.Domain.Common;

namespace DeductionService.Domain.Entities;

/// <summary>
/// Maps to DEDUCTION_ACCESS table — access control for deduction operations.
/// </summary>
public class DeductionAccess : BaseEntity
{
    public long? AccessNumber { get; private set; }   // DE_UNT_ACC
    public long? UnitCode { get; private set; }       // DE_COM_COD
    public string? DeductionType { get; private set; }// DE_DED_TYP (3)
    public decimal? SystemId { get; private set; }    // DE_SYS_ID
    public decimal? EnteredByUserId { get; private set; } // DE_ENT_USR
    public DateTime? EnteredOn { get; private set; }  // DE_ENT_ON
    public DateTime? ClosedOn { get; private set; }   // DE_CLS_DAT

    private DeductionAccess() { }

    public static DeductionAccess Grant(
        long accessNumber,
        long unitCode,
        string deductionType,
        decimal systemId,
        decimal enteredByUserId)
    {
        var access = new DeductionAccess
        {
            AccessNumber = accessNumber,
            UnitCode = unitCode,
            DeductionType = deductionType,
            SystemId = systemId,
            EnteredByUserId = enteredByUserId,
            EnteredOn = DateTime.UtcNow
        };

        access.AddDomainEvent(new Events.DeductionAccessGrantedEvent(accessNumber, unitCode, deductionType));
        return access;
    }

    public void Revoke()
    {
        if (ClosedOn.HasValue)
            throw new Exceptions.DeductionDomainException("Access is already revoked.");

        ClosedOn = DateTime.UtcNow;
    }

    public bool IsActive => ClosedOn == null;
}
