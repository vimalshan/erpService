using EmployeeService.Domain.Common;
using EmployeeService.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace EmployeeService.Domain.Entities
{
    /// <summary>
    /// Employee Aggregate Root - Core entity representing an employee in the system
    /// </summary>
    public class Employee : BaseEntity
    {
        // Embedded value objects
        public PersonalInfo PersonalInfo { get; private set; }
        public ContactInfo ContactInfo { get; private set; }
        public EmploymentDetails EmploymentDetails { get; private set; }
        public GradeInfo GradeInfo { get; private set; }
        public OrganizationalAssignment OrganizationalAssignment { get; private set; }
        public SalaryInfo SalaryInfo { get; private set; }

        // Additional properties
        public long? PINNumber { get; set; }
        public string Status { get; set; } = "ACTIVE"; // ACTIVE, INACTIVE, TERMINATED
        public string Salutation { get; set; }
        public bool IsTerminated { get; set; } = false;
        public string TerminationFlag { get; set; } // BLT, CLT, etc.
        public string ProcessType { get; set; }
        public string InclusionStatus { get; set; }

        // Financial Year Reference
        public long? FinancialYearId { get; set; }

        // Collections
        public ICollection<EmployeeAccountability> Accountabilities { get; set; } = new List<EmployeeAccountability>();
        public ICollection<EmployeeAppraisal> Appraisals { get; set; } = new List<EmployeeAppraisal>();
        public ICollection<EmployeeCareerPlan> CareerPlans { get; set; } = new List<EmployeeCareerPlan>();
        public ICollection<EmployeeBenefit> Benefits { get; set; } = new List<EmployeeBenefit>();

        public Employee() { }

        /// <summary>
        /// Create new employee
        /// </summary>
        public static Employee Create(
            PersonalInfo personalInfo,
            ContactInfo contactInfo,
            EmploymentDetails employmentDetails,
            GradeInfo gradeInfo,
            OrganizationalAssignment organizationalAssignment,
            SalaryInfo salaryInfo,
            long? pinNumber = null,
            string salutation = null)
        {
            var employee = new Employee
            {
                PersonalInfo = personalInfo,
                ContactInfo = contactInfo,
                EmploymentDetails = employmentDetails,
                GradeInfo = gradeInfo,
                OrganizationalAssignment = organizationalAssignment,
                SalaryInfo = salaryInfo,
                PINNumber = pinNumber,
                Salutation = salutation,
                Status = "ACTIVE",
                CreatedOn = DateTime.UtcNow
            };

            return employee;
        }

        /// <summary>
        /// Update employee basic information
        /// </summary>
        public void UpdatePersonalInfo(PersonalInfo personalInfo)
        {
            PersonalInfo = personalInfo;
            ModifiedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// Update contact information
        /// </summary>
        public void UpdateContactInfo(ContactInfo contactInfo)
        {
            ContactInfo = contactInfo;
            ModifiedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// Update organizational assignment
        /// </summary>
        public void UpdateOrganizationalAssignment(OrganizationalAssignment assignment)
        {
            OrganizationalAssignment = assignment;
            ModifiedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// Update salary information
        /// </summary>
        public void UpdateSalary(SalaryInfo salaryInfo)
        {
            SalaryInfo = salaryInfo;
            ModifiedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// Update grade information
        /// </summary>
        public void UpdateGrade(GradeInfo gradeInfo)
        {
            GradeInfo = gradeInfo;
            ModifiedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// Terminate employee
        /// </summary>
        public void Terminate(string terminationFlag, DateTime exitDate, long? modifiedBy = null)
        {
            IsTerminated = true;
            TerminationFlag = terminationFlag;
            EmploymentDetails.ExitDate = exitDate;
            Status = "TERMINATED";
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// Reactivate terminated employee
        /// </summary>
        public void Reactivate(long? modifiedBy = null)
        {
            IsTerminated = false;
            TerminationFlag = null;
            EmploymentDetails.ExitDate = null;
            Status = "ACTIVE";
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// Get employee age
        /// </summary>
        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - PersonalInfo.DateOfBirth.Year;
            if (PersonalInfo.DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }

        /// <summary>
        /// Get years of service
        /// </summary>
        public int GetYearsOfService()
        {
            var today = DateTime.Today;
            var years = today.Year - EmploymentDetails.JoiningDate.Year;
            if (EmploymentDetails.JoiningDate.Date > today.AddYears(-years)) years--;
            return years;
        }

        /// <summary>
        /// Get full display name
        /// </summary>
        public string GetDisplayName()
        {
            return $"{PersonalInfo.GetFullName()} ({EmploymentDetails.EmployeeNumber})";
        }
    }
}
