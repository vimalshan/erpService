using MediatR;
using System;
using System.Collections.Generic;

namespace EmployeeService.Application.Queries.Employees
{
    // ==================== Get Queries ====================

    /// <summary>
    /// Query to get employee by ID
    /// </summary>
    public class GetEmployeeByIdQuery : IRequest<EmployeeDto>
    {
        public long EmployeeId { get; set; }
    }

    /// <summary>
    /// Query to get employee by employee number
    /// </summary>
    public class GetEmployeeByNumberQuery : IRequest<EmployeeDto>
    {
        public string EmployeeNumber { get; set; }
    }

    /// <summary>
    /// Query to get employee by user ID
    /// </summary>
    public class GetEmployeeByUserIdQuery : IRequest<EmployeeDto>
    {
        public string UserId { get; set; }
    }

    /// <summary>
    /// Query to get all active employees
    /// </summary>
    public class GetAllActiveEmployeesQuery : IRequest<List<EmployeeDto>>
    {
    }

    /// <summary>
    /// Query to get employees by unit
    /// </summary>
    public class GetEmployeesByUnitQuery : IRequest<List<EmployeeDto>>
    {
        public long UnitId { get; set; }
    }

    /// <summary>
    /// Query to get employees by grade
    /// </summary>
    public class GetEmployeesByGradeQuery : IRequest<List<EmployeeDto>>
    {
        public string GradeCode { get; set; }
    }

    /// <summary>
    /// Query to get employees by designation
    /// </summary>
    public class GetEmployeesByDesignationQuery : IRequest<List<EmployeeDto>>
    {
        public string Designation { get; set; }
    }

    /// <summary>
    /// Query to search employees
    /// </summary>
    public class SearchEmployeesQuery : IRequest<List<EmployeeDto>>
    {
        public string SearchTerm { get; set; }
    }

    /// <summary>
    /// Query to get employee statistics
    /// </summary>
    public class GetEmployeeStatisticsQuery : IRequest<EmployeeStatisticsDto>
    {
    }

    /// <summary>
    /// Query to get employees with details
    /// </summary>
    public class GetEmployeeWithDetailsQuery : IRequest<EmployeeDetailedDto>
    {
        public long EmployeeId { get; set; }
    }

    /// <summary>
    /// Query to get employees for appraisal
    /// </summary>
    public class GetEmployeesForAppraisalQuery : IRequest<List<EmployeeDto>>
    {
        public long FinancialYearId { get; set; }
    }

    /// <summary>
    /// Query to get employees by reporting manager
    /// </summary>
    public class GetEmployeesByManagerQuery : IRequest<List<EmployeeDto>>
    {
        public long ManagerEmployeeId { get; set; }
    }

    // ==================== DTOs ====================

    /// <summary>
    /// Employee DTO - for basic employee information
    /// </summary>
    public class EmployeeDto
    {
        public long EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
        public string GradeCode { get; set; }
        public string GradeName { get; set; }
        public string Unit { get; set; }
        public DateTime JoiningDate { get; set; }
        public int YearsOfService { get; set; }
        public string Status { get; set; }
        public bool IsTerminated { get; set; }
    }

    /// <summary>
    /// Detailed Employee DTO - includes all details
    /// </summary>
    public class EmployeeDetailedDto
    {
        public long EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string UserId { get; set; }

        // Personal Information
        public PersonalInfoDto PersonalInfo { get; set; }

        // Contact Information
        public ContactInfoDto ContactInfo { get; set; }

        // Employment Details
        public EmploymentDetailsDto EmploymentDetails { get; set; }

        // Grade Information
        public GradeInfoDto GradeInfo { get; set; }

        // Organizational Assignment
        public OrganizationalAssignmentDto OrganizationalAssignment { get; set; }

        // Salary Information
        public SalaryInfoDto SalaryInfo { get; set; }

        // Status
        public string Status { get; set; }
        public bool IsTerminated { get; set; }

        // Related Data
        public List<AppraisalSummaryDto> Appraisals { get; set; }
        public List<CareerPlanSummaryDto> CareerPlans { get; set; }
        public List<BenefitSummaryDto> Benefits { get; set; }
    }

    public class PersonalInfoDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
    }

    public class ContactInfoDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternatePhone { get; set; }
    }

    public class EmploymentDetailsDto
    {
        public string EmployeeNumber { get; set; }
        public string UserId { get; set; }
        public string NickName { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ExitDate { get; set; }
    }

    public class GradeInfoDto
    {
        public string GradeCode { get; set; }
        public string GradeName { get; set; }
        public long GradeId { get; set; }
        public string CadreName { get; set; }
        public string GradeType { get; set; }
    }

    public class OrganizationalAssignmentDto
    {
        public long UnitBusinessId { get; set; }
        public long UnitOrgId { get; set; }
        public string UnitCode { get; set; }
        public string Unit { get; set; }
        public string Designation { get; set; }
        public string HRRoleId { get; set; }
    }

    public class SalaryInfoDto
    {
        public decimal BasicSalary { get; set; }
        public string SalaryType { get; set; }
    }

    public class AppraisalSummaryDto
    {
        public long AppraisalId { get; set; }
        public long FinancialYearId { get; set; }
        public string Status { get; set; }
        public decimal? PerformanceScore { get; set; }
        public DateTime AppraisalDate { get; set; }
    }

    public class CareerPlanSummaryDto
    {
        public long CareerPlanId { get; set; }
        public string CareerPath { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class BenefitSummaryDto
    {
        public long BenefitId { get; set; }
        public string BenefitName { get; set; }
        public string BenefitCode { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// Employee Statistics DTO
    /// </summary>
    public class EmployeeStatisticsDto
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int TerminatedEmployees { get; set; }
        public decimal AverageSalary { get; set; }
        public Dictionary<string, int> EmployeesByGrade { get; set; }
        public Dictionary<string, int> EmployeesByUnit { get; set; }
        public Dictionary<string, int> EmployeesByDesignation { get; set; }
        public int EmployeesWithConfirmedAppraisal { get; set; }
        public int EmployeesPendingCareerPlanning { get; set; }
    }

    /// <summary>
    /// Pagination DTO
    /// </summary>
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResult(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
