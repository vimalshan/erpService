using EmployeeService.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;

namespace EmployeeService.Application.Commands.Employees
{
    // ==================== Create Commands ====================

    /// <summary>
    /// Command to create new employee
    /// </summary>
    public class CreateEmployeeCommand : IRequest<CreateEmployeeResponse>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternatePhone { get; set; }
        public string EmployeeNumber { get; set; }
        public string UserId { get; set; }
        public string NickName { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string GradeCode { get; set; }
        public string GradeName { get; set; }
        public long GradeId { get; set; }
        public string CadreName { get; set; }
        public long UnitBusinessId { get; set; }
        public long UnitOrgId { get; set; }
        public string UnitCode { get; set; }
        public string Unit { get; set; }
        public string Designation { get; set; }
        public string HRRoleId { get; set; }
        public decimal BasicSalary { get; set; }
        public string SalaryType { get; set; }
        public string Salutation { get; set; }
        public long? PINNumber { get; set; }
    }

    public class CreateEmployeeResponse
    {
        public long EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    // ==================== Update Commands ====================

    /// <summary>
    /// Command to update employee personal information
    /// </summary>
    public class UpdateEmployeePersonalInfoCommand : IRequest<BaseResponse>
    {
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; }
        public string Salutation { get; set; }
    }

    /// <summary>
    /// Command to update employee contact information
    /// </summary>
    public class UpdateEmployeeContactCommand : IRequest<BaseResponse>
    {
        public long EmployeeId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternatePhone { get; set; }
    }

    /// <summary>
    /// Command to update employee organizational assignment
    /// </summary>
    public class UpdateEmployeeOrganizationalAssignmentCommand : IRequest<BaseResponse>
    {
        public long EmployeeId { get; set; }
        public long UnitBusinessId { get; set; }
        public long UnitOrgId { get; set; }
        public string UnitCode { get; set; }
        public string Unit { get; set; }
        public string Designation { get; set; }
        public string HRRoleId { get; set; }
    }

    /// <summary>
    /// Command to update employee salary
    /// </summary>
    public class UpdateEmployeeSalaryCommand : IRequest<BaseResponse>
    {
        public long EmployeeId { get; set; }
        public decimal BasicSalary { get; set; }
        public string SalaryType { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    /// <summary>
    /// Command to update employee grade
    /// </summary>
    public class UpdateEmployeeGradeCommand : IRequest<BaseResponse>
    {
        public long EmployeeId { get; set; }
        public string GradeCode { get; set; }
        public string GradeName { get; set; }
        public long GradeId { get; set; }
        public string CadreName { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    /// <summary>
    /// Command to promote employee
    /// </summary>
    public class PromoteEmployeeCommand : IRequest<BaseResponse>
    {
        public long EmployeeId { get; set; }
        public string FromDesignation { get; set; }
        public string ToDesignation { get; set; }
        public string FromGrade { get; set; }
        public string ToGrade { get; set; }
        public long NewGradeId { get; set; }
        public decimal NewBasicSalary { get; set; }
        public DateTime PromotionDate { get; set; }
    }

    /// <summary>
    /// Command to terminate employee
    /// </summary>
    public class TerminateEmployeeCommand : IRequest<BaseResponse>
    {
        public long EmployeeId { get; set; }
        public string TerminationFlag { get; set; } // BLT, CLT
        public DateTime ExitDate { get; set; }
        public string Reason { get; set; }
    }

    /// <summary>
    /// Command to reactivate employee
    /// </summary>
    public class ReactivateEmployeeCommand : IRequest<BaseResponse>
    {
        public long EmployeeId { get; set; }
        public string Reason { get; set; }
    }

    /// <summary>
    /// Command to transfer employee
    /// </summary>
    public class TransferEmployeeCommand : IRequest<BaseResponse>
    {
        public long EmployeeId { get; set; }
        public long FromUnitId { get; set; }
        public long ToUnitId { get; set; }
        public string FromUnit { get; set; }
        public string ToUnit { get; set; }
        public DateTime TransferDate { get; set; }
    }

    /// <summary>
    /// Command to delete employee (soft delete)
    /// </summary>
    public class DeleteEmployeeCommand : IRequest<BaseResponse>
    {
        public long EmployeeId { get; set; }
    }

    // ==================== Base Response ====================

    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
