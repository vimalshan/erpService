using System;
using EmployeeService.Domain.Common;
using EmployeeService.Domain.ValueObjects;
using EmployeeService.Domain.Events;

namespace EmployeeService.Domain.Entities;

/// <summary>
/// Employee aggregate root entity
/// </summary>
public class Employee : BaseEntity
{
    public long EmployeeSystemId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string? CostCenterId { get; set; }
    
    // CTC Information
    public Money GrossCTC { get; set; } = new(0);
    public Money BasicSalary { get; set; } = new(0);
    public DateTime? CTCEffectiveDate { get; set; }
    
    // Employment Status
    public string EmploymentStatus { get; set; } = "Active";
    public DateTime JoiningDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    
    // Audit Fields
    public DateTime LastCTCModificationDate { get; set; }

    #region Constructors

    private Employee() { }

    public Employee(
        long employeeSystemId,
        string firstName,
        string lastName,
        string email,
        string employeeCode,
        DateTime joiningDate)
    {
        if (employeeSystemId <= 0)
            throw new ArgumentException("Employee System ID must be greater than 0", nameof(employeeSystemId));

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First Name is required", nameof(firstName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        EmployeeSystemId = employeeSystemId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        EmployeeCode = employeeCode;
        JoiningDate = joiningDate;
        LastCTCModificationDate = DateTime.UtcNow;
    }

    #endregion

    #region Business Methods

    /// <summary>
    /// Update employee's CTC with an increment percentage
    /// </summary>
    public void IncrementCTC(Percentage incrementPercentage, DateTime effectiveDate, long approvedBy)
    {
        if (string.IsNullOrEmpty(EmploymentStatus) || EmploymentStatus != "Active")
            throw new InvalidOperationException("Can only increment CTC for active employees");

        if (GrossCTC.Amount == 0)
            throw new InvalidOperationException("Employee must have existing CTC before increment");

        if (effectiveDate < DateTime.UtcNow.Date)
            throw new ArgumentException("Effective date cannot be in the past", nameof(effectiveDate));

        var oldCTC = GrossCTC;
        var newCTCAmount = incrementPercentage.ApplyPercentage(GrossCTC.Amount);
        var newBasicSalaryAmount = incrementPercentage.ApplyPercentage(BasicSalary.Amount);

        GrossCTC = new Money(newCTCAmount);
        BasicSalary = new Money(newBasicSalaryAmount);
        CTCEffectiveDate = effectiveDate;
        UpdatedAt = DateTime.UtcNow;
        LastCTCModificationDate = DateTime.UtcNow;

        var domainEvent = new EmployeeCTCIncrementedEvent(
            EmployeeSystemId,
            oldCTC,
            GrossCTC,
            incrementPercentage,
            effectiveDate,
            approvedBy);

        AddDomainEvent(domainEvent);
    }

    /// <summary>
    /// Modify employee CTC directly (for special cases)
    /// </summary>
    public void ModifyCTC(Money newGrossCTC, Money newBasicSalary, DateTime effectiveDate, string reason, long modifiedBy)
    {
        if (string.IsNullOrEmpty(EmploymentStatus) || EmploymentStatus != "Active")
            throw new InvalidOperationException("Can only modify CTC for active employees");

        if (newGrossCTC.Amount <= 0 || newBasicSalary.Amount <= 0)
            throw new ArgumentException("CTC amounts must be greater than zero");

        if (newBasicSalary.Amount > newGrossCTC.Amount)
            throw new ArgumentException("Basic salary cannot exceed gross CTC");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason for CTC modification is required", nameof(reason));

        var oldGrossCTC = GrossCTC;
        var oldBasicSalary = BasicSalary;

        GrossCTC = newGrossCTC;
        BasicSalary = newBasicSalary;
        CTCEffectiveDate = effectiveDate;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = modifiedBy.ToString();
        LastCTCModificationDate = DateTime.UtcNow;

        var domainEvent = new EmployeeCTCModifiedEvent(
            EmployeeSystemId,
            oldGrossCTC,
            newGrossCTC,
            oldBasicSalary,
            newBasicSalary,
            effectiveDate,
            reason);

        AddDomainEvent(domainEvent);
    }

    /// <summary>
    /// Initialize CTC for new employee
    /// </summary>
    public void InitializeCTC(Money grossCTC, Money basicSalary, DateTime effectiveDate)
    {
        if (GrossCTC.Amount != 0)
            throw new InvalidOperationException("CTC is already initialized for this employee");

        if (grossCTC.Amount <= 0 || basicSalary.Amount <= 0)
            throw new ArgumentException("CTC amounts must be greater than zero");

        if (basicSalary.Amount > grossCTC.Amount)
            throw new ArgumentException("Basic salary cannot exceed gross CTC");

        GrossCTC = grossCTC;
        BasicSalary = basicSalary;
        CTCEffectiveDate = effectiveDate;
        LastCTCModificationDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Terminate employee contract
    /// </summary>
    public void Terminate(DateTime terminationDate)
    {
        if (terminationDate < DateTime.UtcNow.Date)
            throw new ArgumentException("Termination date cannot be in the past", nameof(terminationDate));

        EmploymentStatus = "Terminated";
        TerminationDate = terminationDate;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Update employee personal information
    /// </summary>
    public void UpdatePersonalInformation(string firstName, string lastName, string? middleName, string email, string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First Name is required", nameof(firstName));

        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        Email = email;
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Update cost center assignment
    /// </summary>
    public void AssignCostCenter(string costCenterId)
    {
        if (string.IsNullOrWhiteSpace(costCenterId))
            throw new ArgumentException("Cost Center ID is required", nameof(costCenterId));

        CostCenterId = costCenterId;
        UpdatedAt = DateTime.UtcNow;
    }

    #endregion

    #region Query Methods

    /// <summary>
    /// Get full name of employee
    /// </summary>
    public string GetFullName()
    {
        return string.IsNullOrEmpty(MiddleName)
            ? $"{FirstName} {LastName}"
            : $"{FirstName} {MiddleName} {LastName}";
    }

    /// <summary>
    /// Calculate years of service
    /// </summary>
    public int GetYearsOfService()
    {
        var endDate = TerminationDate ?? DateTime.UtcNow;
        return (endDate.Year - JoiningDate.Year) - (endDate < JoiningDate.AddYears(1) ? 1 : 0);
    }

    /// <summary>
    /// Check if employee can receive increment
    /// </summary>
    public bool CanReceiveIncrement()
    {
        return !IsDeleted 
            && EmploymentStatus == "Active" 
            && GrossCTC.Amount > 0
            && TerminationDate == null;
    }

    #endregion
}
