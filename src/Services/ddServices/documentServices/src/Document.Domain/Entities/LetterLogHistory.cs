using Document.Domain.Common;

namespace Document.Domain.Entities;

/// <summary>
/// Maps to DDLETTER_LOGHISTORY — tracks when letters are opened/accessed.
/// </summary>
public class LetterLogHistory : BaseEntity
{
    public decimal LogSysId { get; private set; }
    public string IpAddress { get; private set; } = default!;
    public DateTime OpenedOn { get; private set; }
    public decimal? FinancialYearId { get; private set; }
    public decimal? EmployeeSysId { get; private set; }
    public string? LetterType { get; private set; }

    private LetterLogHistory() { }

    public static LetterLogHistory Create(
        decimal logSysId,
        string ipAddress,
        decimal? employeeSysId,
        string? letterType,
        decimal? financialYearId = null)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            throw new Exceptions.DomainException("IP address is required for letter log.");

        return new LetterLogHistory
        {
            LogSysId = logSysId,
            IpAddress = ipAddress,
            OpenedOn = DateTime.UtcNow,
            EmployeeSysId = employeeSysId,
            LetterType = letterType,
            FinancialYearId = financialYearId
        };
    }
}
