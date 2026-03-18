namespace Document.Domain.ValueObjects;

public sealed class SignatoryImage : Common.ValueObject
{
    public string FileName { get; }

    private SignatoryImage(string fileName) => FileName = fileName;

    public static SignatoryImage Of(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new Exceptions.DomainException("Signatory image filename cannot be empty.");
        return new SignatoryImage(fileName);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FileName;
    }

    public override string ToString() => FileName;
}
