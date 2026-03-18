using EmployeeService.Application.Commands.Employees;
using HotChocolate;
using MediatR;

namespace EmployeeService.API.GraphQL
{
    /// <summary>
    /// GraphQL Mutation Type - Define write operations for employee data.
    /// </summary>
    public class Mutation
    {
        public async Task<CreateEmployeeResponse> CreateEmployeeAsync(CreateEmployeeInput input, [Service] IMediator mediatr)
        {
            var command = new CreateEmployeeCommand
            {
                FirstName = input.FirstName,
                MiddleName = input.MiddleName,
                LastName = input.LastName,
                DateOfBirth = input.DateOfBirth,
                Gender = string.IsNullOrWhiteSpace(input.Gender) ? '\0' : input.Gender.Trim()[0],
                Email = input.Email,
                PhoneNumber = input.PhoneNumber,
                AlternatePhone = input.AlternatePhone,
                EmployeeNumber = input.EmployeeNumber,
                UserId = input.UserId,
                NickName = input.NickName,
                JoiningDate = input.JoiningDate,
                EffectiveDate = input.EffectiveDate,
                ConfirmationDate = input.ConfirmationDate,
                GradeCode = input.GradeCode,
                GradeName = input.GradeName,
                GradeId = input.GradeId,
                CadreName = input.CadreName,
                UnitBusinessId = input.UnitBusinessId,
                UnitOrgId = input.UnitOrgId,
                UnitCode = input.UnitCode,
                Unit = input.Unit,
                Designation = input.Designation,
                HRRoleId = input.HRRoleId,
                BasicSalary = input.BasicSalary,
                SalaryType = input.SalaryType,
                Salutation = input.Salutation,
                PINNumber = input.PINNumber
            };

            return await mediatr.Send(command);
        }

        public async Task<BaseResponse> UpdateEmployeeSalaryAsync(long employeeId, UpdateEmployeeSalaryInput input, [Service] IMediator mediatr)
        {
            var command = new UpdateEmployeeSalaryCommand
            {
                EmployeeId = employeeId,
                BasicSalary = input.BasicSalary,
                SalaryType = input.SalaryType,
                EffectiveDate = input.EffectiveDate
            };

            return await mediatr.Send(command);
        }

        public async Task<BaseResponse> PromoteEmployeeAsync(long employeeId, PromoteEmployeeInput input, [Service] IMediator mediatr)
        {
            var command = new PromoteEmployeeCommand
            {
                EmployeeId = employeeId,
                FromDesignation = input.FromDesignation,
                ToDesignation = input.ToDesignation,
                FromGrade = input.FromGrade,
                ToGrade = input.ToGrade,
                NewGradeId = input.NewGradeId,
                NewBasicSalary = input.NewBasicSalary,
                PromotionDate = input.PromotionDate
            };

            return await mediatr.Send(command);
        }

        public async Task<BaseResponse> TerminateEmployeeAsync(long employeeId, TerminateEmployeeInput input, [Service] IMediator mediatr)
        {
            var command = new TerminateEmployeeCommand
            {
                EmployeeId = employeeId,
                TerminationFlag = input.TerminationFlag,
                ExitDate = input.ExitDate,
                Reason = input.Reason
            };

            return await mediatr.Send(command);
        }
    }

    public class CreateEmployeeInput
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
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

    public class UpdateEmployeeSalaryInput
    {
        public decimal BasicSalary { get; set; }
        public string SalaryType { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    public class PromoteEmployeeInput
    {
        public string FromDesignation { get; set; }
        public string ToDesignation { get; set; }
        public string FromGrade { get; set; }
        public string ToGrade { get; set; }
        public long NewGradeId { get; set; }
        public decimal NewBasicSalary { get; set; }
        public DateTime PromotionDate { get; set; }
    }

    public class TerminateEmployeeInput
    {
        public string TerminationFlag { get; set; }
        public DateTime ExitDate { get; set; }
        public string Reason { get; set; }
    }
}