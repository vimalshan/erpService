using Document.Domain.Common;

namespace Document.Domain.Entities;

/// <summary>
/// Maps to DD_SIGNATORY — master signatory information used on generated letters.
/// </summary>
public class Signatory : BaseEntity
{
    public decimal SignatoryNumber { get; private set; }
    public string? Name { get; private set; }
    public string? Designation { get; private set; }
    public string? LiveFlag { get; private set; }
    public decimal? EmployeeSysId { get; private set; }
    public string? ImageFileName { get; private set; }
    public string? DigitalSignPfxFileName { get; private set; }
    public string? DigitalSignPfxPassword { get; private set; }
    public string? AlternateImageFileName { get; private set; }

    private Signatory() { }

    public static Signatory Create(
        decimal signatoryNumber,
        string name,
        string designation,
        decimal? employeeSysId = null,
        string? imageFileName = null)
    {
        var signatory = new Signatory
        {
            SignatoryNumber = signatoryNumber,
            Name = name,
            Designation = designation,
            EmployeeSysId = employeeSysId,
            ImageFileName = imageFileName,
            LiveFlag = "Y"
        };
        signatory.AddDomainEvent(new Events.SignatoryCreatedEvent(signatory));
        return signatory;
    }

    public void Update(string name, string designation, string? imageFileName = null)
    {
        Name = name;
        Designation = designation;
        ImageFileName = imageFileName ?? ImageFileName;
        AddDomainEvent(new Events.SignatoryUpdatedEvent(this));
    }

    public void Deactivate() => LiveFlag = "N";
    public void Activate() => LiveFlag = "Y";
}
