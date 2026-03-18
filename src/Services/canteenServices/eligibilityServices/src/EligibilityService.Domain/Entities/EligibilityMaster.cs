using EligibilityService.Domain.Common;
using EligibilityService.Domain.ValueObjects;

namespace EligibilityService.Domain.Entities;

/// <summary>Maps to CAN_ELIGIBILITY_MASTER table.</summary>
public class EligibilityMaster : BaseEntity
{
    public long CanteenUnit { get; private set; }           // CN_COM_COD
    public string ShiftCode { get; private set; } = default!; // CN_SFT_COD
    public decimal ItemCode { get; private set; }            // CN_ITM_COD
    public int? EligibleLimit { get; private set; }          // CN_ELG_LMT
    public long? EnteredUser { get; private set; }           // CN_ENT_USR
    public DateTime? EnteredOn { get; private set; }         // CN_ENT_DAT
    public string? TimeOfficeUnit { get; private set; }      // CN_TIM_UNT

    private EligibilityMaster() { }

    public static EligibilityMaster Create(
        long canteenUnit,
        string shiftCode,
        decimal itemCode,
        int? eligibleLimit,
        long? enteredUser,
        string? timeOfficeUnit)
    {
        var entity = new EligibilityMaster
        {
            CanteenUnit = canteenUnit,
            ShiftCode = shiftCode,
            ItemCode = itemCode,
            EligibleLimit = eligibleLimit,
            EnteredUser = enteredUser,
            EnteredOn = DateTime.UtcNow,
            TimeOfficeUnit = timeOfficeUnit
        };

        entity.RaiseDomainEvent(new Events.EligibilityCreatedEvent(entity));
        return entity;
    }

    public void Update(int? eligibleLimit, string? timeOfficeUnit, long modifiedUser)
    {
        EligibleLimit = eligibleLimit;
        TimeOfficeUnit = timeOfficeUnit;
        RaiseDomainEvent(new Events.EligibilityUpdatedEvent(this, modifiedUser));
    }
}
