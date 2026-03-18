using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

public sealed class EmployeeRetiral : BaseEntity
{
    public long EmployeeId { get; private set; }
    public long TransactionId { get; private set; }
    public char? PfApplicable { get; private set; }
    public string? PfTrust { get; private set; }
    public long? PfNo { get; private set; }
    public char? AdditionalPf { get; private set; }
    public long? AdditionalPfPercent { get; private set; }
    public char? GratuityApplicable { get; private set; }
    public char? SuperannuationApplicable { get; private set; }
    public char? SuperannuationOption { get; private set; }
    public long? SuperannuationNo { get; private set; }
    public char? EsiApplicable { get; private set; }
    public long? EsiNo { get; private set; }
    public DateTime? EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private EmployeeRetiral() { }

    public static EmployeeRetiral Create(long employeeId, long transactionId,
        char? pfApplicable, string? pfTrust, long? pfNo, char? gratuityApplicable,
        char? esiApplicable, long? esiNo, DateTime? effectiveDate, long updatedBy)
    {
        return new EmployeeRetiral
        {
            EmployeeId = employeeId, TransactionId = transactionId, PfApplicable = pfApplicable,
            PfTrust = pfTrust, PfNo = pfNo, GratuityApplicable = gratuityApplicable,
            EsiApplicable = esiApplicable, EsiNo = esiNo, EffectiveDate = effectiveDate,
            UpdatedBy = updatedBy, UpdatedOn = DateTime.UtcNow
        };
    }
}
