using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Events;
using EmployeeManagement.Domain.ValueObjects;

namespace EmployeeManagement.Domain.Entities;

/// <summary>Aggregate root representing an employee and their related data.</summary>
public sealed class Employee : AggregateRoot
{
    // Navigation collections — populated by repository
    private readonly List<EmployeeQualification> _qualifications = new();
    private readonly List<EmployeeCareer> _careers = new();
    private readonly List<EmployeeLanguage> _languages = new();
    private readonly List<EmployeeDiary> _diaries = new();
    private readonly List<EmployeePromotion> _promotions = new();
    private readonly List<EmployeeTransfer> _transfers = new();

    public string? EmployeeNo { get; private set; }
    public string? BusinessUnit { get; private set; }
    public string? Unit { get; private set; }
    public long? GradeId { get; private set; }
    public string? Designation { get; private set; }
    public long? DivisionId { get; private set; }
    public long? DepartmentId { get; private set; }
    public long? PositionId { get; private set; }
    public bool IsActive { get; private set; }

    public EmployeeAddress? CurrentAddress { get; private set; }
    public EmployeeAddress? PermanentAddress { get; private set; }
    public EmployeeProbation? Probation { get; private set; }
    public EmployeeRetiral? Retiral { get; private set; }

    public IReadOnlyCollection<EmployeeQualification> Qualifications => _qualifications.AsReadOnly();
    public IReadOnlyCollection<EmployeeCareer> Careers => _careers.AsReadOnly();
    public IReadOnlyCollection<EmployeeLanguage> Languages => _languages.AsReadOnly();
    public IReadOnlyCollection<EmployeeDiary> Diaries => _diaries.AsReadOnly();
    public IReadOnlyCollection<EmployeePromotion> Promotions => _promotions.AsReadOnly();
    public IReadOnlyCollection<EmployeeTransfer> Transfers => _transfers.AsReadOnly();

    // EF constructor
    private Employee() { }

    public static Employee Create(long id, string employeeNo, string businessUnit, string unit,
        long gradeId, string designation, long divisionId, long departmentId, long positionId,
        long createdBy)
    {
        var employee = new Employee
        {
            Id = id,
            EmployeeNo = employeeNo,
            BusinessUnit = businessUnit,
            Unit = unit,
            GradeId = gradeId,
            Designation = designation,
            DivisionId = divisionId,
            DepartmentId = departmentId,
            PositionId = positionId,
            IsActive = true
        };
        employee.SetAudit(createdBy);
        employee.AddDomainEvent(new EmployeeCreatedEvent(id, employeeNo, createdBy));
        return employee;
    }

    public void UpdateDesignation(string designation, long updatedBy)
    {
        Designation = designation;
        UpdateAudit(updatedBy);
    }

    public void Promote(long newGradeId, long newPositionId, long promotionNo, long promotedBy)
    {
        var oldGradeId = GradeId ?? 0;
        GradeId = newGradeId;
        PositionId = newPositionId;
        UpdateAudit(promotedBy);
        AddDomainEvent(new EmployeePromotedEvent(Id, promotionNo, oldGradeId, newGradeId, promotedBy));
    }

    public void Transfer(string newUnit, long newUnitId, long transferId, long transferredBy)
    {
        var oldUnit = Unit ?? string.Empty;
        Unit = newUnit;
        UpdateAudit(transferredBy);
        AddDomainEvent(new EmployeeTransferredEvent(Id, transferId, oldUnit, newUnit, transferredBy));
    }

    public void Deactivate(long deactivatedBy)
    {
        IsActive = false;
        UpdateAudit(deactivatedBy);
    }
}
