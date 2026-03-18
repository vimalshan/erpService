using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

public sealed class EmployeeLanguage : BaseEntity
{
    public long EmployeeId { get; private set; }
    public long LanguageId { get; private set; }
    public string? LanguageType { get; private set; }  // SRW = Speak, Read, Write
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private EmployeeLanguage() { }

    public static EmployeeLanguage Create(long employeeId, long languageId, string? languageType, long updatedBy)
        => new() { EmployeeId = employeeId, LanguageId = languageId, LanguageType = languageType, UpdatedBy = updatedBy, UpdatedOn = DateTime.UtcNow };
}
