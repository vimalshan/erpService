using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

public sealed class EmployeeAddress : BaseEntity
{
    public long EmployeeId { get; private set; }
    public char AddressFlag { get; private set; }  // C = Current, P = Permanent
    public string? Line1 { get; private set; }
    public string? Line2 { get; private set; }
    public string? Line3 { get; private set; }
    public string? Line4 { get; private set; }
    public long? CityId { get; private set; }
    public string? CityOthers { get; private set; }
    public long? PinCode { get; private set; }
    public long? StateId { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private EmployeeAddress() { }

    public static EmployeeAddress Create(long employeeId, char flag, string? line1, string? line2,
        string? line3, string? line4, long? cityId, string? cityOthers, long? pinCode, long? stateId,
        long updatedBy)
    {
        return new EmployeeAddress
        {
            EmployeeId = employeeId,
            AddressFlag = flag,
            Line1 = line1, Line2 = line2, Line3 = line3, Line4 = line4,
            CityId = cityId, CityOthers = cityOthers, PinCode = pinCode, StateId = stateId,
            UpdatedBy = updatedBy, UpdatedOn = DateTime.UtcNow
        };
    }
}
