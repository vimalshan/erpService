using LetTransactionService.Domain.Common;
using LetTransactionService.Domain.Events;
using LetTransactionService.Domain.Exceptions;

namespace LetTransactionService.Domain.Entities;

/// <summary>Maps to LET_MAIN table — core LET request header.</summary>
public class LetMain : BaseEntity
{
    public long RequestNumber { get; private set; }
    public int FinancialYearSerialNo { get; private set; }
    public string EmployeeUserId { get; private set; } = string.Empty;
    public string? SupervisorUserId { get; private set; }
    public DateTime? RequestDate { get; private set; }

    private readonly List<LetSub> _subEntries = [];
    public IReadOnlyList<LetSub> SubEntries => _subEntries.AsReadOnly();

    private LetMain() { }

    public static LetMain Create(
        long requestNumber,
        int financialYearSerialNo,
        string employeeUserId,
        string? supervisorUserId,
        DateTime? requestDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(employeeUserId);

        var entity = new LetMain
        {
            RequestNumber = requestNumber,
            FinancialYearSerialNo = financialYearSerialNo,
            EmployeeUserId = employeeUserId,
            SupervisorUserId = supervisorUserId,
            RequestDate = requestDate ?? DateTime.UtcNow
        };

        entity.AddDomainEvent(new LetRequestCreatedEvent(requestNumber, employeeUserId));
        return entity;
    }

    public LetSub AddSubEntry(
        int serialNumber,
        char? preferredModeDev,
        string? actionTaken,
        int? courseId,
        string? trainingProgramBhr,
        string? impactBenefitProcess,
        string? measureCompetency,
        int? competencyToDevelop,
        string? domainKnowledgeDev,
        string? domainKnowledgeDevDetail,
        string? processDev,
        string? processDevDetail,
        char? letSubCode,
        string? reviewType)
    {
        var sub = LetSub.Create(
            RequestNumber, serialNumber, preferredModeDev, actionTaken,
            courseId, trainingProgramBhr, impactBenefitProcess, measureCompetency,
            competencyToDevelop, domainKnowledgeDev, domainKnowledgeDevDetail,
            processDev, processDevDetail, letSubCode, reviewType);

        _subEntries.Add(sub);
        AddDomainEvent(new LetRequestSubAddedEvent(RequestNumber, serialNumber));
        return sub;
    }

    public void UpdateSubEntry(
        int serialNumber,
        string? midYearReviewerName,
        string? midYearReviewerDate,
        string? midYearReviewerRemark,
        string? annualReviewerName,
        string? annualReviewerDate,
        string? annualReviewerRemark)
    {
        var sub = _subEntries.FirstOrDefault(s => s.SerialNumber == serialNumber)
            ?? throw new LetDomainException($"Sub-entry with serial number {serialNumber} not found.");

        sub.UpdateReviews(midYearReviewerName, midYearReviewerDate, midYearReviewerRemark,
            annualReviewerName, annualReviewerDate, annualReviewerRemark);
        AddDomainEvent(new LetRequestSubUpdatedEvent(RequestNumber, serialNumber));
    }
}
