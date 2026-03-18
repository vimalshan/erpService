using MediatR;
using System;

namespace EmployeeService.Domain.Events
{
    /// <summary>
    /// Base domain event class
    /// </summary>
    public abstract class BaseDomainEvent : INotification
    {
        public long EmployeeId { get; set; }
        public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
        public long? TriggeredBy { get; set; }
    }

    /// <summary>
    /// Event raised when employee is created
    /// </summary>
    public class EmployeeCreatedEvent : BaseDomainEvent
    {
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime JoiningDate { get; set; }
    }

    /// <summary>
    /// Event raised when employee personal info is updated
    /// </summary>
    public class EmployeePersonalInfoUpdatedEvent : BaseDomainEvent
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UpdateDetails { get; set; }
    }

    /// <summary>
    /// Event raised when employee contact info is updated
    /// </summary>
    public class EmployeeContactInfoUpdatedEvent : BaseDomainEvent
    {
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    /// <summary>
    /// Event raised when employee is terminated
    /// </summary>
    public class EmployeeTerminatedEvent : BaseDomainEvent
    {
        public string TerminationReason { get; set; }
        public DateTime ExitDate { get; set; }
        public string TerminationFlag { get; set; }
    }

    /// <summary>
    /// Event raised when employee is reactivated
    /// </summary>
    public class EmployeeReactivatedEvent : BaseDomainEvent
    {
        public DateTime ReactivationDate { get; set; }
    }

    /// <summary>
    /// Event raised when employee salary is updated
    /// </summary>
    public class EmployeeSalaryUpdatedEvent : BaseDomainEvent
    {
        public decimal OldBasicSalary { get; set; }
        public decimal NewBasicSalary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    /// <summary>
    /// Event raised when employee grade is updated
    /// </summary>
    public class EmployeeGradeUpdatedEvent : BaseDomainEvent
    {
        public string OldGrade { get; set; }
        public string NewGrade { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    /// <summary>
    /// Event raised when employee is promoted
    /// </summary>
    public class EmployeePromotedEvent : BaseDomainEvent
    {
        public string FromDesignation { get; set; }
        public string ToDesignation { get; set; }
        public string FromGrade { get; set; }
        public string ToGrade { get; set; }
        public DateTime PromotionDate { get; set; }
    }

    /// <summary>
    /// Event raised when employee is transferred
    /// </summary>
    public class EmployeeTransferredEvent : BaseDomainEvent
    {
        public string FromUnit { get; set; }
        public long FromUnitId { get; set; }
        public string ToUnit { get; set; }
        public long ToUnitId { get; set; }
        public DateTime TransferDate { get; set; }
    }

    /// <summary>
    /// Event raised when appraisal is submitted
    /// </summary>
    public class AppraisalSubmittedEvent : BaseDomainEvent
    {
        public long AppraisalId { get; set; }
        public long FinancialYearId { get; set; }
        public DateTime SubmissionDate { get; set; }
    }

    /// <summary>
    /// Event raised when appraisal is approved
    /// </summary>
    public class AppraisalApprovedEvent : BaseDomainEvent
    {
        public long AppraisalId { get; set; }
        public decimal PerformanceScore { get; set; }
        public DateTime ApprovalDate { get; set; }
    }

    /// <summary>
    /// Event raised when career plan is created
    /// </summary>
    public class CareerPlanCreatedEvent : BaseDomainEvent
    {
        public long CareerPlanId { get; set; }
        public string CareerPath { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// Event raised when benefit is allocated
    /// </summary>
    public class BenefitAllocatedEvent : BaseDomainEvent
    {
        public long BenefitId { get; set; }
        public string BenefitName { get; set; }
        public decimal Amount { get; set; }
        public long FinancialYearId { get; set; }
    }

    /// <summary>
    /// Event raised when benefit is withdrawn
    /// </summary>
    public class BenefitWithdrawnEvent : BaseDomainEvent
    {
        public long BenefitId { get; set; }
        public string BenefitName { get; set; }
        public DateTime WithdrawalDate { get; set; }
    }

    /// <summary>
    /// Event raised when accountability is assigned
    /// </summary>
    public class AccountabilityAssignedEvent : BaseDomainEvent
    {
        public long AccountabilityId { get; set; }
        public long PositionId { get; set; }
        public string Description { get; set; }
        public DateTime AssignmentDate { get; set; }
    }
}
