using VisitorServices.Domain.Enums;

namespace VisitorServices.Domain.ValueObjects;

public sealed class IdDocument
{
    public IdType IdType { get; }
    public string? IdNumber { get; }

    private IdDocument() { }

    public IdDocument(IdType idType, string? idNumber)
    {
        IdType = idType;
        IdNumber = idNumber?.Trim();
    }

    public static IdDocument Create(char idTypeChar, string? idNumber)
    {
        var idType = idTypeChar switch
        {
            'N' => IdType.NationalId,
            'P' => IdType.Passport,
            'D' => IdType.DriverLicense,
            _ => IdType.Other
        };
        return new IdDocument(idType, idNumber);
    }

    public char ToChar() => (char)(int)IdType;

    public override bool Equals(object? obj) =>
        obj is IdDocument other && IdType == other.IdType && IdNumber == other.IdNumber;

    public override int GetHashCode() => HashCode.Combine(IdType, IdNumber);
}
