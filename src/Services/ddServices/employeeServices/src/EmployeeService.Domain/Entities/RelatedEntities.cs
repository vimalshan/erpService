using EmployeeService.Domain.Common;
using System;
using System.Collections.Generic;

namespace EmployeeService.Domain.Entities
{
    /// <summary>
    /// Employee Accountability - represents responsibilities assigned to an employee in a position
    /// </summary>
    public class EmployeeAccountability : BaseEntity
    {
        public long EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public long PositionId { get; set; }
        public string Description { get; set; }
        public bool IsClosed { get; set; } = false;

        public EmployeeAccountability() { }

        public static EmployeeAccountability Create(long employeeId, long positionId, string description)
        {
            return new EmployeeAccountability
            {
                EmployeeId = employeeId,
                PositionId = positionId,
                Description = description,
                IsClosed = false,
                CreatedOn = DateTime.UtcNow
            };
        }

        public void Close(long closedBy)
        {
            IsClosed = true;
            ModifiedBy = closedBy;
            ModifiedOn = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Employee Appraisal - represents performance appraisal records
    /// </summary>
    public class EmployeeAppraisal : BaseEntity
    {
        public long EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public long FinancialYearId { get; set; }
        public long? AppraiserEmployeeId { get; set; }
        public long? SupervisorEmployeeId { get; set; }
        public string Status { get; set; } = "DRAFT"; // DRAFT, SUBMITTED, APPROVED, REJECTED
        public DateTime AppraisalDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public DateTime? ApprovalDate { get; set; }

        // Appraisal data
        public decimal? PerformanceScore { get; set; }
        public string Comments { get; set; }
        public string AppraiseeComments { get; set; }
        public string AppraiseeDiscussion { get; set; }

        public ICollection<AppraisalObjective> Objectives { get; set; } = new List<AppraisalObjective>();
        public ICollection<AppraisalCompetency> Competencies { get; set; } = new List<AppraisalCompetency>();

        public EmployeeAppraisal() { }

        public static EmployeeAppraisal Create(long employeeId, long financialYearId, long appraiserEmployeeId)
        {
            return new EmployeeAppraisal
            {
                EmployeeId = employeeId,
                FinancialYearId = financialYearId,
                AppraiserEmployeeId = appraiserEmployeeId,
                Status = "DRAFT",
                AppraisalDate = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow
            };
        }

        public void Submit(long submittedBy)
        {
            Status = "SUBMITTED";
            SubmissionDate = DateTime.UtcNow;
            ModifiedBy = submittedBy;
            ModifiedOn = DateTime.UtcNow;
        }

        public void Approve(long approvedBy)
        {
            Status = "APPROVED";
            ApprovalDate = DateTime.UtcNow;
            ModifiedBy = approvedBy;
            ModifiedOn = DateTime.UtcNow;
        }

        public void Reject(long rejectedBy)
        {
            Status = "REJECTED";
            ModifiedBy = rejectedBy;
            ModifiedOn = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Appraisal Objective - represents objectives set for an appraisal
    /// </summary>
    public class AppraisalObjective : BaseEntity
    {
        public long AppraisalId { get; set; }
        public EmployeeAppraisal Appraisal { get; set; }
        public string Objective { get; set; }
        public string Description { get; set; }
        public decimal? TargetValue { get; set; }
        public decimal? AchievedValue { get; set; }
        public decimal? WeightagePercentage { get; set; }
    }

    /// <summary>
    /// Appraisal Competency - represents competencies assessed in an appraisal
    /// </summary>
    public class AppraisalCompetency : BaseEntity
    {
        public long AppraisalId { get; set; }
        public EmployeeAppraisal Appraisal { get; set; }
        public string CompetencyName { get; set; }
        public string CompetencyCode { get; set; }
        public decimal? RatingScore { get; set; }
        public decimal? WeightagePercentage { get; set; }
        public string Comments { get; set; }
    }

    /// <summary>
    /// Employee Career Plan - represents career development and planning
    /// </summary>
    public class EmployeeCareerPlan : BaseEntity
    {
        public long EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public long? FinancialYearId { get; set; }
        public long? VersionNumber { get; set; }

        // Career promotion details
        public string CareerPath { get; set; } // Path for promotion
        public bool IsPromotionPosition { get; set; }
        public string ProposedSuccessor { get; set; }
        public int ProposedSuccessorPeriodMonths { get; set; }

        // Career development milestones
        public string Milestone1 { get; set; }
        public string Milestone2 { get; set; }
        public string Milestone3 { get; set; }
        public string Milestone4 { get; set; }
        public string Milestone5 { get; set; }
        public string Milestone6 { get; set; }

        // Own aspiration
        public bool OwnAspiration { get; set; }
        public string ConstraintRemarks { get; set; }
        public string ProposalRemarks { get; set; }
        public string SuspensionReason { get; set; }
        public int SuspensionPeriodMonths { get; set; }

        public string Status { get; set; } = "DRAFT"; // DRAFT, SUBMITTED, APPROVED, ACTIVE
        public DateTime? CreatedDate { get; set; }

        public EmployeeCareerPlan() { }

        public static EmployeeCareerPlan Create(long employeeId, long? financialYearId = null)
        {
            return new EmployeeCareerPlan
            {
                EmployeeId = employeeId,
                FinancialYearId = financialYearId,
                Status = "DRAFT",
                CreatedDate = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow
            };
        }

        public void Submit(long submittedBy)
        {
            Status = "SUBMITTED";
            ModifiedBy = submittedBy;
            ModifiedOn = DateTime.UtcNow;
        }

        public void Approve(long approvedBy)
        {
            Status = "APPROVED";
            ModifiedBy = approvedBy;
            ModifiedOn = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Employee Benefit - represents benefits assigned or received by an employee
    /// </summary>
    public class EmployeeBenefit : BaseEntity
    {
        public long EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public long FinancialYearId { get; set; }
        public string BenefitCode { get; set; }
        public string BenefitName { get; set; }
        public string BenefitDescription { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "ACTIVE"; // ACTIVE, INACTIVE, CLAIMED, PENDING
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Remarks { get; set; }

        public EmployeeBenefit() { }

        public static EmployeeBenefit Create(long employeeId, long financialYearId, string benefitCode, string benefitName, decimal amount)
        {
            return new EmployeeBenefit
            {
                EmployeeId = employeeId,
                FinancialYearId = financialYearId,
                BenefitCode = benefitCode,
                BenefitName = benefitName,
                Amount = amount,
                Status = "ACTIVE",
                EffectiveDate = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow
            };
        }

        public void Deactivate(long deactivatedBy)
        {
            Status = "INACTIVE";
            ExpiryDate = DateTime.UtcNow;
            ModifiedBy = deactivatedBy;
            ModifiedOn = DateTime.UtcNow;
        }

        public void UpdateAmount(decimal newAmount, long modifiedBy)
        {
            Amount = newAmount;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.UtcNow;
        }
    }
}
