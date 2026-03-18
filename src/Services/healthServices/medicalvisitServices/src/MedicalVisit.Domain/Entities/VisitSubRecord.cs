using MedicalVisit.Domain.Common;

namespace MedicalVisit.Domain.Entities;

public class VisitSubRecord : BaseEntity
{
    public string CompanyCode { get; private set; } = null!;
    public long VisitNumber { get; private set; }
    public string? TestType { get; private set; }
    public string? TestValue { get; private set; }
    public long? SerialNumber { get; private set; }

    private VisitSubRecord() { }

    public static VisitSubRecord Create(
        string companyCode,
        long visitNumber,
        string? testType,
        string? testValue,
        long? serialNumber = null)
    {
        if (testType?.Length > 20)
            throw new ArgumentException("Test type cannot exceed 20 characters", nameof(testType));

        if (testValue?.Length > 25)
            throw new ArgumentException("Test value cannot exceed 25 characters", nameof(testValue));

        return new VisitSubRecord
        {
            CompanyCode = companyCode,
            VisitNumber = visitNumber,
            TestType = testType,
            TestValue = testValue,
            SerialNumber = serialNumber
        };
    }

    public void UpdateTestValue(string newTestValue)
    {
        if (newTestValue?.Length > 25)
            throw new ArgumentException("Test value cannot exceed 25 characters", nameof(newTestValue));

        TestValue = newTestValue;
    }
}
