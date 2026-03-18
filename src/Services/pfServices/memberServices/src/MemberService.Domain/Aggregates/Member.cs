using MemberService.Domain.Common;
using MemberService.Domain.Entities;
using MemberService.Domain.Enums;
using MemberService.Domain.Events;
using MemberService.Domain.Exceptions;
using MemberService.Domain.ValueObjects;

namespace MemberService.Domain.Aggregates;

/// <summary>
/// Member aggregate root — owns Nominees, Payroll records and Contacts.
/// </summary>
public class Member : BaseEntity
{
    private readonly List<MemberNominee> _nominees = new();
    private readonly List<MemberPayroll> _payrollRecords = new();
    private readonly List<MemberContact> _contacts = new();

    // ── Identifiers ──────────────────────────────────────────────
    public long MemberNo { get; private set; }
    public string TrustCode { get; private set; } = string.Empty;
    public string FpsTrustCode { get; private set; } = string.Empty;
    public int OpfNo { get; private set; }
    public int PensionNo { get; private set; }

    // ── Personal ─────────────────────────────────────────────────
    public string MemberName { get; private set; } = string.Empty;
    public string? FatherName { get; private set; }
    public DateTime? DateOfBirth { get; private set; }

    // ── Employment ───────────────────────────────────────────────
    public DateTime EnrollmentDate { get; private set; }
    public DateTime DateOfJoining { get; private set; }
    public string EmployeeType { get; private set; } = string.Empty;
    public string UnitCode { get; private set; } = string.Empty;
    public long EmployeeNo { get; private set; }
    public long EmployeeSysId { get; private set; }

    // ── Enrollment metadata ──────────────────────────────────────
    public string EnrollUserId { get; private set; } = string.Empty;
    public long EnrollSysId { get; private set; }
    public DateTime EnrollDate { get; private set; }

    // ── Closure ──────────────────────────────────────────────────
    public DateTime? ClosureDate { get; private set; }
    public DateTime? LeaveDate { get; private set; }
    public string? LeaveReason { get; private set; }

    // ── Status ───────────────────────────────────────────────────
    public MemberStatus Status { get; private set; } = MemberStatus.Active;
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    // ── Navigation ───────────────────────────────────────────────
    public IReadOnlyCollection<MemberNominee> Nominees => _nominees.AsReadOnly();
    public IReadOnlyCollection<MemberPayroll> PayrollRecords => _payrollRecords.AsReadOnly();
    public IReadOnlyCollection<MemberContact> Contacts => _contacts.AsReadOnly();

    private Member() { }

    // ── Factory ──────────────────────────────────────────────────
    public static Member Create(long memberNo, string memberName, string trustCode, DateTime dateOfJoining,
        DateTime? dateOfBirth, string employeeType, long employeeSysId, string unitCode,
        long employeeNo, long createdBy)
    {
        if (string.IsNullOrWhiteSpace(memberName))
            throw new MemberDomainException("Member name is required.");
        if (string.IsNullOrWhiteSpace(trustCode))
            throw new MemberDomainException("Trust code is required.");

        var member = new Member
        {
            MemberNo = memberNo,
            TrustCode = trustCode,
            FpsTrustCode = trustCode,
            OpfNo = (int)memberNo,
            PensionNo = (int)memberNo,
            MemberName = memberName,
            DateOfJoining = dateOfJoining,
            DateOfBirth = dateOfBirth,
            EmployeeType = employeeType,
            EmployeeSysId = employeeSysId,
            UnitCode = unitCode,
            EmployeeNo = employeeNo,
            EnrollUserId = createdBy.ToString(),
            EnrollSysId = createdBy,
            EnrollDate = DateTime.UtcNow,
            EnrollmentDate = DateTime.UtcNow,
            Status = MemberStatus.Active
        };

        member._payrollRecords.Add(MemberPayroll.Create(memberNo, unitCode, employeeNo, dateOfJoining));
        member.RaiseDomainEvent(MemberCreatedEvent.Create(memberNo, memberName, trustCode, createdBy));
        return member;
    }

    // ── Nominee management ───────────────────────────────────────
    public MemberNominee AddNominee(int serialNo, string fundType, string nomineeName,
        string relationshipCode, long percentage, DateTime dob, bool isMinor,
        string? addressLine1 = null, string? phoneNo = null, string? email = null)
    {
        if (Status != MemberStatus.Active)
            throw new MemberDomainException("Cannot add nominee to inactive or closed member.");

        var totalExistingPct = _nominees
            .Where(n => n.Status == NomineeStatus.Active && n.FundType == fundType)
            .Sum(n => n.Percentage);

        if (totalExistingPct + percentage > 100)
            throw new MemberDomainException($"Total nominee percentage for fund type {fundType} would exceed 100%.");

        var nominee = MemberNominee.Create(MemberNo, serialNo, fundType, nomineeName,
            relationshipCode, percentage, dob, isMinor, TrustCode, addressLine1, phoneNo, email);
        _nominees.Add(nominee);

        RaiseDomainEvent(NomineeAddedEvent.Create(MemberNo, serialNo, nomineeName, percentage, fundType));
        return nominee;
    }

    // ── Account closure ──────────────────────────────────────────
    public void CloseAccount(string leaveReason, DateTime leaveDate, long approvedBy)
    {
        if (Status == MemberStatus.Closed)
            throw new MemberDomainException("Member account is already closed.");
        if (string.IsNullOrWhiteSpace(leaveReason))
            throw new MemberDomainException("Leave reason is required to close account.");

        Status = MemberStatus.Closed;
        LeaveReason = leaveReason;
        LeaveDate = leaveDate;
        ClosureDate = DateTime.UtcNow;
        UpdatedBy = approvedBy;
        UpdatedOn = DateTime.UtcNow;

        foreach (var pr in _payrollRecords.Where(p => p.Status == PayrollStatus.Active))
            pr.Close(leaveDate);

        RaiseDomainEvent(MemberClosedEvent.Create(MemberNo, leaveReason, leaveDate, approvedBy));
    }

    // ── Contact management ───────────────────────────────────────
    public MemberContact AddContact(ContactType contactType, string addressLine1,
        string city, string state, string pinCode, string country,
        string? line2 = null, string? line3 = null, string? phone = null, string? email = null)
    {
        var existing = _contacts.FirstOrDefault(c => c.ContactType == contactType && c.ClosureDate == null);
        existing?.Close();

        var contact = MemberContact.Create(MemberNo, contactType, addressLine1, city, state, pinCode, country,
            line2, line3, phone, email);
        _contacts.Add(contact);
        return contact;
    }

    public void UpdateDetails(string? memberName, string? fatherName, DateTime? dob, long updatedBy)
    {
        if (Status != MemberStatus.Active)
            throw new MemberDomainException("Cannot update inactive or closed member.");

        if (!string.IsNullOrWhiteSpace(memberName)) MemberName = memberName;
        FatherName = fatherName ?? FatherName;
        DateOfBirth = dob ?? DateOfBirth;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
