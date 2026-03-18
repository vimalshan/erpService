using Document.Domain.Common;
using Document.Domain.Entities;
using Document.Domain.Events;

namespace Document.Domain.Aggregates;

/// <summary>
/// Document aggregate root — orchestrates appraisal letter generation workflow.
/// </summary>
public class DocumentAggregate : BaseEntity
{
    private readonly List<GeneratedLetter> _generatedLetters = new();
    private readonly List<LetterLogHistory> _logHistory = new();

    public decimal Id { get; private set; }
    public decimal EmployeeSysId { get; private set; }
    public IReadOnlyCollection<GeneratedLetter> GeneratedLetters => _generatedLetters.AsReadOnly();
    public IReadOnlyCollection<LetterLogHistory> LogHistory => _logHistory.AsReadOnly();

    private DocumentAggregate() { }

    public static DocumentAggregate Create(decimal employeeSysId)
        => new() { Id = employeeSysId, EmployeeSysId = employeeSysId };

    public GeneratedLetter GenerateLetter(
        decimal? createdByPin,
        string? employeeName,
        string? letterType,
        DateTime? effectiveDate)
    {
        var letter = GeneratedLetter.Create(createdByPin, EmployeeSysId, employeeName, letterType, effectiveDate);
        _generatedLetters.Add(letter);
        AddDomainEvent(new LetterGeneratedEvent(letter));
        return letter;
    }

    public LetterLogHistory LogAccess(decimal logSysId, string ipAddress, string? letterType, decimal? finYearId = null)
    {
        var log = LetterLogHistory.Create(logSysId, ipAddress, EmployeeSysId, letterType, finYearId);
        _logHistory.Add(log);
        AddDomainEvent(new LetterOpenedEvent(EmployeeSysId, letterType ?? string.Empty, ipAddress));
        return log;
    }
}
