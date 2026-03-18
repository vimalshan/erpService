namespace MedicineManagement.Domain.ValueObjects;

public enum MedicineCategory
{
    High,
    Medium,
    Low
}

public static class MedicineCategoryExtensions
{
    public static char ToCode(this MedicineCategory category) => category switch
    {
        MedicineCategory.High => 'H',
        MedicineCategory.Medium => 'M',
        MedicineCategory.Low => 'L',
        _ => throw new ArgumentOutOfRangeException(nameof(category))
    };

    public static MedicineCategory FromCode(char code) => code switch
    {
        'H' => MedicineCategory.High,
        'M' => MedicineCategory.Medium,
        'L' => MedicineCategory.Low,
        _ => throw new ArgumentException($"Invalid medicine category code: {code}", nameof(code))
    };
}
