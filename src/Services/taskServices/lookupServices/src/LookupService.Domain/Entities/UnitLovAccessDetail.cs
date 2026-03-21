using LookupService.Domain.Common;

namespace LookupService.Domain.Entities;

public class UnitLovAccessDetail : BaseEntity
{
    public decimal UdAccessDetId { get; private set; }
    public decimal? UdAccessMastId { get; private set; }
    public string? UdAccessType { get; private set; }
    public string? UdEmpSysId { get; private set; }
    public decimal? UdEscDays { get; private set; }
    public string? UdEffDat { get; private set; }
    public string? UdClsDat { get; private set; }
    public decimal? UdUpdatedBy { get; private set; }
    public DateTime? UdUpdatedOn { get; private set; }

    // Navigation
    public UnitLovAccessMaster? AccessMaster { get; private set; }

    private UnitLovAccessDetail() { }

    public static UnitLovAccessDetail Create(
        decimal accessDetId,
        decimal accessMastId,
        string accessType,
        string empSysId,
        decimal? escDays = null,
        string? effDat = null,
        string? clsDat = null,
        decimal? updatedBy = null)
    {
        return new UnitLovAccessDetail
        {
            UdAccessDetId = accessDetId,
            UdAccessMastId = accessMastId,
            UdAccessType = accessType,
            UdEmpSysId = empSysId,
            UdEscDays = escDays,
            UdEffDat = effDat,
            UdClsDat = clsDat,
            UdUpdatedBy = updatedBy,
            UdUpdatedOn = DateTime.UtcNow
        };
    }

    public void Update(string accessType, string empSysId, decimal? escDays, string? effDat, string? clsDat, decimal updatedBy)
    {
        UdAccessType = accessType;
        UdEmpSysId = empSysId;
        UdEscDays = escDays;
        UdEffDat = effDat;
        UdClsDat = clsDat;
        UdUpdatedBy = updatedBy;
        UdUpdatedOn = DateTime.UtcNow;
    }
}
