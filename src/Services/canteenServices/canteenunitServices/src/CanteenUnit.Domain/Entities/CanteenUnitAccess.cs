using CanteenUnit.Domain.Common;
using CanteenUnit.Domain.Events;

namespace CanteenUnit.Domain.Entities;

/// <summary>Maps to CANTEEN_UNIT_ACCESS</summary>
public class CanteenUnitAccess : BaseEntity
{
    public long? UnUntAcc { get; private set; }         // UN_UNT_ACC
    public long? UnComCod { get; private set; }         // UN_COM_COD
    public long? UnUsrId { get; private set; }          // UN_USR_ID
    public long? UnEntUsr { get; private set; }         // UN_ENT_USR
    public DateTime? UnEntOn { get; private set; }      // UN_ENT_ON
    public DateTime? UnClsDat { get; private set; }     // UN_CLS_DAT

    private CanteenUnitAccess() { }

    public static CanteenUnitAccess Grant(long accNum, long comCode, long userId, long enteredBy)
    {
        var access = new CanteenUnitAccess
        {
            UnUntAcc = accNum,
            UnComCod = comCode,
            UnUsrId = userId,
            UnEntUsr = enteredBy,
            UnEntOn = DateTime.UtcNow
        };
        access.AddDomainEvent(new CanteenAccessGrantedEvent(comCode, userId, accNum, DateTime.UtcNow));
        return access;
    }

    public void Revoke() => UnClsDat = DateTime.UtcNow;
}
