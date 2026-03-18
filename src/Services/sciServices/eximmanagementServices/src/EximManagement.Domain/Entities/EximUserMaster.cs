using EximManagement.Domain.Common;

namespace EximManagement.Domain.Entities;

public class EximUserMaster : BaseEntity
{
    public long EximUserId { get; private set; }
    public long? EximEmpSysId { get; private set; }
    public string? EximSparshId { get; private set; }
    public DateTime EximUserEffectiveDate { get; private set; }
    public DateTime? EximUserClosureDate { get; private set; }
    public long EximUserEnteredBy { get; private set; }

    private EximUserMaster() { }

    public static EximUserMaster Create(long userId, long? empSysId, string? sparshId, DateTime effectiveDate, long enteredBy)
        => new()
        {
            EximUserId = userId,
            EximEmpSysId = empSysId,
            EximSparshId = sparshId,
            EximUserEffectiveDate = effectiveDate,
            EximUserEnteredBy = enteredBy
        };

    public void Close(DateTime closureDate) => EximUserClosureDate = closureDate;
}
