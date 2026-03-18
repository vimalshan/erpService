namespace EmployeeManagement.Domain.ValueObjects;

/// <summary>Represents an education period in MMYYYY format.</summary>
public sealed class EducationPeriod
{
    public string? From { get; }  // MMYYYY
    public string? To { get; }    // MMYYYY

    public EducationPeriod(string? from, string? to)
    {
        From = from;
        To = to;
    }
}
