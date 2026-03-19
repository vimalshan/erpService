using VehicleTracking.Domain.Common;
using VehicleTracking.Domain.Events;
using VehicleTracking.Domain.ValueObjects;

namespace VehicleTracking.Domain.Entities;

public class VehicleMaster : AuditableEntity
{
    public long SerialNumber { get; set; }
    public string RegNum1 { get; set; } = string.Empty;
    public string? RegNum2 { get; set; }
    public string? RegNum3 { get; set; }
    public string RegNum4 { get; set; } = string.Empty;
    public DateTime? RegistrationDate { get; set; }
    public string? LogUser { get; set; }
    public long? LogNumber { get; set; }
    public DateTime? LogDate { get; set; }

    public RegistrationNumber GetRegistrationNumber()
        => RegistrationNumber.Create(RegNum1, RegNum2, RegNum3, RegNum4);

    public static VehicleMaster Register(string regNum1, string? regNum2, string? regNum3, string regNum4,
        DateTime? regDate, string updatedBy, long updatedByNum)
    {
        var vehicle = new VehicleMaster
        {
            RegNum1 = regNum1,
            RegNum2 = regNum2,
            RegNum3 = regNum3,
            RegNum4 = regNum4,
            RegistrationDate = regDate,
            UpdatedBy = updatedBy,
            UpdateNumber = updatedByNum,
            UpdatedDate = DateTime.UtcNow
        };

        vehicle.AddDomainEvent(new VehicleRegisteredEvent(vehicle));
        return vehicle;
    }
}
