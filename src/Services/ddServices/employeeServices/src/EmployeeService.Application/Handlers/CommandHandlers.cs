using AutoMapper;
using EmployeeService.Application.Commands.Employees;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.Events;
using EmployeeService.Domain.Repositories;
using EmployeeService.Domain.ValueObjects;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Application.Handlers.Commands
{
    /// <summary>
    /// Handler for CreateEmployeeCommand
    /// </summary>
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, CreateEmployeeResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateEmployeeResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if employee number is unique
                if (await _unitOfWork.Employees.IsEmployeeNumberUniqueAsync(request.EmployeeNumber))
                {
                    return new CreateEmployeeResponse
                    {
                        Success = false,
                        Message = "Employee number already exists."
                    };
                }

                // Check if user ID is unique
                if (!await _unitOfWork.Employees.IsUserIdUniqueAsync(request.UserId))
                {
                    return new CreateEmployeeResponse
                    {
                        Success = false,
                        Message = "User ID already exists."
                    };
                }

                // Create value objects
                var personalInfo = new PersonalInfo(
                    request.FirstName,
                    request.LastName,
                    request.DateOfBirth,
                    request.Gender,
                    request.MiddleName);

                var contactInfo = new ContactInfo(
                    request.Email,
                    request.PhoneNumber,
                    request.AlternatePhone);

                var employmentDetails = new EmploymentDetails(
                    request.EmployeeNumber,
                    request.UserId,
                    request.NickName,
                    request.JoiningDate,
                    request.EffectiveDate)
                {
                    ConfirmationDate = request.ConfirmationDate
                };

                var gradeInfo = new GradeInfo(
                    request.GradeCode,
                    request.GradeName,
                    request.GradeId,
                    request.CadreName);

                var organizationalAssignment = new OrganizationalAssignment(
                    request.UnitBusinessId,
                    request.UnitOrgId,
                    request.UnitCode,
                    request.Unit,
                    request.Designation,
                    request.HRRoleId);

                var salaryInfo = new SalaryInfo(
                    request.BasicSalary,
                    request.SalaryType);

                // Create employee aggregate
                var employee = Employee.Create(
                    personalInfo,
                    contactInfo,
                    employmentDetails,
                    gradeInfo,
                    organizationalAssignment,
                    salaryInfo,
                    request.PINNumber,
                    request.Salutation);

                // Add domain event
                var @event = new EmployeeCreatedEvent
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.PersonalInfo.GetFullName(),
                    EmployeeNumber = employee.EmploymentDetails.EmployeeNumber,
                    JoiningDate = employee.EmploymentDetails.JoiningDate
                };

                employee.AddDomainEvent(@event);

                // Save employee
                await _unitOfWork.Employees.AddAsync(employee);
                await _unitOfWork.SaveChangesAsync();

                return new CreateEmployeeResponse
                {
                    EmployeeId = employee.Id,
                    EmployeeNumber = employee.EmploymentDetails.EmployeeNumber,
                    Message = "Employee created successfully.",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new CreateEmployeeResponse
                {
                    Success = false,
                    Message = $"Error creating employee: {ex.Message}"
                };
            }
        }
    }

    /// <summary>
    /// Handler for UpdateEmployeePersonalInfoCommand
    /// </summary>
    public class UpdateEmployeePersonalInfoCommandHandler : IRequestHandler<UpdateEmployeePersonalInfoCommand, BaseResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeePersonalInfoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(UpdateEmployeePersonalInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                var personalInfo = new PersonalInfo(
                    request.FirstName,
                    request.LastName,
                    request.DateOfBirth,
                    request.Gender,
                    request.MiddleName);

                employee.UpdatePersonalInfo(personalInfo);
                employee.Salutation = request.Salutation;

                // Add domain event
                var @event = new EmployeePersonalInfoUpdatedEvent
                {
                    EmployeeId = employee.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UpdateDetails = "Personal information updated"
                };

                employee.AddDomainEvent(@event);

                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse
                {
                    Success = true,
                    Message = "Employee personal information updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Error updating employee: {ex.Message}"
                };
            }
        }
    }

    /// <summary>
    /// Handler for UpdateEmployeeContactCommand
    /// </summary>
    public class UpdateEmployeeContactCommandHandler : IRequestHandler<UpdateEmployeeContactCommand, BaseResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeContactCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(UpdateEmployeeContactCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                var contactInfo = new ContactInfo(
                    request.Email,
                    request.PhoneNumber,
                    request.AlternatePhone);

                employee.UpdateContactInfo(contactInfo);

                // Add domain event
                var @event = new EmployeeContactInfoUpdatedEvent
                {
                    EmployeeId = employee.Id,
                    Email = request.Email,
                    Phone = request.PhoneNumber
                };

                employee.AddDomainEvent(@event);

                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse
                {
                    Success = true,
                    Message = "Employee contact information updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Error updating employee: {ex.Message}"
                };
            }
        }
    }

    /// <summary>
    /// Handler for TerminateEmployeeCommand
    /// </summary>
    public class TerminateEmployeeCommandHandler : IRequestHandler<TerminateEmployeeCommand, BaseResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TerminateEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(TerminateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                if (employee.IsTerminated)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Employee is already terminated."
                    };
                }

                employee.Terminate(request.TerminationFlag, request.ExitDate);

                // Add domain event
                var @event = new EmployeeTerminatedEvent
                {
                    EmployeeId = employee.Id,
                    TerminationReason = request.Reason,
                    ExitDate = request.ExitDate,
                    TerminationFlag = request.TerminationFlag
                };

                employee.AddDomainEvent(@event);

                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse
                {
                    Success = true,
                    Message = "Employee terminated successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Error terminating employee: {ex.Message}"
                };
            }
        }
    }
}
