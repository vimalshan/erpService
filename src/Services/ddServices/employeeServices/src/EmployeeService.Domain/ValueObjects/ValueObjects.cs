using System;

namespace EmployeeService.Domain.ValueObjects
{
    /// <summary>
    /// Value object for employee contact information
    /// </summary>
    public class ContactInfo
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternatePhone { get; set; }

        public ContactInfo(string email, string phoneNumber, string alternatePhone = null)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            AlternatePhone = alternatePhone;
        }

        // Parameterless constructor for EF Core value object deserialization
        public ContactInfo() { }
    }

    /// <summary>
    /// Value object for employee personal information
    /// </summary>
    public class PersonalInfo
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; } // M/F

        public PersonalInfo(string firstName, string lastName, DateTime dateOfBirth, char gender, string middleName = null)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            MiddleName = middleName;
        }

        // Parameterless constructor for EF Core value object deserialization
        public PersonalInfo() { }

        public string GetFullName()
        {
            return string.IsNullOrEmpty(MiddleName) 
                ? $"{FirstName} {LastName}" 
                : $"{FirstName} {MiddleName} {LastName}";
        }
    }

    /// <summary>
    /// Value object for employment details
    /// </summary>
    public class EmploymentDetails
    {
        public string EmployeeNumber { get; set; }
        public string UserId { get; set; }
        public string NickName { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ExitDate { get; set; }

        public EmploymentDetails(string employeeNumber, string userId, string nickName, DateTime joiningDate, DateTime effectiveDate)
        {
            EmployeeNumber = employeeNumber;
            UserId = userId;
            NickName = nickName;
            JoiningDate = joiningDate;
            EffectiveDate = effectiveDate;
        }

        // Parameterless constructor for EF Core value object deserialization
        public EmploymentDetails() { }
    }

    /// <summary>
    /// Value object for grade and cadre
    /// </summary>
    public class GradeInfo
    {
        public string GradeCode { get; set; }
        public string GradeName { get; set; }
        public long GradeId { get; set; }
        public string CadreName { get; set; }
        public string GradeType { get; set; }

        public GradeInfo(string gradeCode, string gradeName, long gradeId, string cadreName, string gradeType = null)
        {
            GradeCode = gradeCode;
            GradeName = gradeName;
            GradeId = gradeId;
            CadreName = cadreName;
            GradeType = gradeType;
        }

        // Parameterless constructor for EF Core value object deserialization
        public GradeInfo() { }
    }

    /// <summary>
    /// Value object for organizational assignment
    /// </summary>
    public class OrganizationalAssignment
    {
        public long UnitBusinessId { get; set; }
        public long UnitOrgId { get; set; }
        public string UnitCode { get; set; }
        public string Unit { get; set; }
        public string Designation { get; set; }
        public string HRRoleId { get; set; }
        public long? CurrentLevelId { get; set; }

        public OrganizationalAssignment(long unitBusinessId, long unitOrgId, string unitCode, string unit, string designation, string hrRoleId)
        {
            UnitBusinessId = unitBusinessId;
            UnitOrgId = unitOrgId;
            UnitCode = unitCode;
            Unit = unit;
            Designation = designation;
            HRRoleId = hrRoleId;
        }

        // Parameterless constructor for EF Core value object deserialization
        public OrganizationalAssignment() { }
    }

    /// <summary>
    /// Value object for salary information
    /// </summary>
    public class SalaryInfo
    {
        public decimal BasicSalary { get; set; }
        public string SalaryType { get; set; } // CT, BT, etc.
        public decimal? CurrentLevel { get; set; }

        public SalaryInfo(decimal basicSalary, string salaryType)
        {
            BasicSalary = basicSalary;
            SalaryType = salaryType;
        }

        // Parameterless constructor for EF Core value object deserialization
        public SalaryInfo() { }
    }
}
