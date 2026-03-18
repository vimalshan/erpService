using MedicineManagement.Domain.Common;

namespace MedicineManagement.Domain.Entities;

public class DoctorAttendant : BaseEntity, IAggregateRoot
{
    public string? Code { get; private set; }
    public char? Flag { get; private set; } // D=Doctor, A=Attendant
    public string? Name { get; private set; }
    public long? SystemId { get; private set; }

    private DoctorAttendant() { }

    public static DoctorAttendant Create(string? code, char? flag, string? name)
    {
        return new DoctorAttendant
        {
            Code = code,
            Flag = flag,
            Name = name
        };
    }

    public bool IsDoctor() => Flag == 'D';
    public bool IsAttendant() => Flag == 'A';
}
