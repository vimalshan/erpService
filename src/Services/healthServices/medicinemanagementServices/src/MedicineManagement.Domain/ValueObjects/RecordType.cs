namespace MedicineManagement.Domain.ValueObjects;

public enum RecordType
{
    OpeningBalance,
    Purchase,
    Issue,
    Expire
}

public static class RecordTypeExtensions
{
    public static char ToCode(this RecordType type) => type switch
    {
        RecordType.OpeningBalance => 'O',
        RecordType.Purchase => 'P',
        RecordType.Issue => 'I',
        RecordType.Expire => 'E',
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    public static RecordType FromCode(char code) => code switch
    {
        'O' => RecordType.OpeningBalance,
        'P' => RecordType.Purchase,
        'I' => RecordType.Issue,
        'E' => RecordType.Expire,
        _ => throw new ArgumentException($"Invalid record type code: {code}", nameof(code))
    };
}
