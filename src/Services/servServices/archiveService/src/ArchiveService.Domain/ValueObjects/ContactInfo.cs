using ArchiveService.Domain.Common;

namespace ArchiveService.Domain.ValueObjects;

public class ContactInfo : ValueObject
{
    public string? ContactNo { get; private set; }
    public string? AltContactNo { get; private set; }

    private ContactInfo() { }

    public ContactInfo(string? contactNo, string? altContactNo)
    {
        ContactNo = contactNo;
        AltContactNo = altContactNo;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ContactNo;
        yield return AltContactNo;
    }
}
