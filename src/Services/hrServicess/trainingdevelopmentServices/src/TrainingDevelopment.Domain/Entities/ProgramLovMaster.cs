using TrainingDevelopment.Domain.Common;

namespace TrainingDevelopment.Domain.Entities;

/// <summary>
/// Maps to PROGRAMLOV_MAST table — Program List of Values master.
/// </summary>
public class ProgramLovMaster : BaseEntity
{
    public string TypeCode { get; private set; } = default!;   // PRLOV_TYPECODE (PK)
    public string Code { get; private set; } = default!;       // PRLOV_CODE
    public string Name { get; private set; } = default!;       // PRLOV_NAME

    private ProgramLovMaster() { }

    public static ProgramLovMaster Create(string typeCode, string code, string name)
    {
        return new ProgramLovMaster
        {
            TypeCode = typeCode,
            Code = code,
            Name = name
        };
    }

    public void Update(string code, string name)
    {
        Code = code;
        Name = name;
    }
}
