using EmployeeService.Application.Commands.Employees;
using EmployeeService.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Application.Handlers.Commands
{
    /// <summary>
    /// Handler for UpdateEmployeeOrganizationalAssignmentCommand
    /// </summary>
    public class UpdateEmployeeOrganizationalAssignmentCommandHandler : IRequestHandler<UpdateEmployeeOrganizationalAssignmentCommand, BaseResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeOrganizationalAssignmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(UpdateEmployeeOrganizationalAssignmentCommand request, CancellationToken cancellationToken)
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

                var assignment = new Domain.ValueObjects.OrganizationalAssignment(
                    request.UnitBusinessId,
                    request.UnitOrgId,
                    request.UnitCode,
                    request.Unit,
                    request.Designation,
                    request.HRRoleId);

                employee.UpdateOrganizationalAssignment(assignment);

                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse
                {
                    Success = true,
                    Message = "Employee organizational assignment updated successfully."
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
    /// Handler for UpdateEmployeeSalaryCommand
    /// </summary>
    public class UpdateEmployeeSalaryCommandHandler : IRequestHandler<UpdateEmployeeSalaryCommand, BaseResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeSalaryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(UpdateEmployeeSalaryCommand request, CancellationToken cancellationToken)
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

                var oldSalary = employee.SalaryInfo.BasicSalary;
                var salaryInfo = new Domain.ValueObjects.SalaryInfo(request.BasicSalary, request.SalaryType);
                employee.UpdateSalary(salaryInfo);

                // Add domain event
                var @event = new Domain.Events.EmployeeSalaryUpdatedEvent
                {
                    EmployeeId = employee.Id,
                    OldBasicSalary = oldSalary,
                    NewBasicSalary = request.BasicSalary,
                    EffectiveDate = request.EffectiveDate
                };

                employee.AddDomainEvent(@event);

                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse
                {
                    Success = true,
                    Message = "Employee salary updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Error updating employee salary: {ex.Message}"
                };
            }
        }
    }

    /// <summary>
    /// Handler for UpdateEmployeeGradeCommand
    /// </summary>
    public class UpdateEmployeeGradeCommandHandler : IRequestHandler<UpdateEmployeeGradeCommand, BaseResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeGradeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(UpdateEmployeeGradeCommand request, CancellationToken cancellationToken)
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

                var oldGrade = employee.GradeInfo.GradeCode;
                var gradeInfo = new Domain.ValueObjects.GradeInfo(
                    request.GradeCode,
                    request.GradeName,
                    request.GradeId,
                    request.CadreName);

                employee.UpdateGrade(gradeInfo);

                // Add domain event
                var @event = new Domain.Events.EmployeeGradeUpdatedEvent
                {
                    EmployeeId = employee.Id,
                    OldGrade = oldGrade,
                    NewGrade = request.GradeCode,
                    EffectiveDate = request.EffectiveDate
                };

                employee.AddDomainEvent(@event);

                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse
                {
                    Success = true,
                    Message = "Employee grade updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Error updating employee grade: {ex.Message}"
                };
            }
        }
    }

    /// <summary>
    /// Handler for PromoteEmployeeCommand
    /// </summary>
    public class PromoteEmployeeCommandHandler : IRequestHandler<PromoteEmployeeCommand, BaseResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PromoteEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(PromoteEmployeeCommand request, CancellationToken cancellationToken)
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

                // Update grade
                var gradeInfo = new Domain.ValueObjects.GradeInfo(
                    request.ToGrade,
                    request.ToGrade,
                    request.NewGradeId,
                    employee.GradeInfo.CadreName);

                employee.UpdateGrade(gradeInfo);

                // Update salary
                var salaryInfo = new Domain.ValueObjects.SalaryInfo(
                    request.NewBasicSalary,
                    employee.SalaryInfo.SalaryType);

                employee.UpdateSalary(salaryInfo);

                // Update designation
                var assignment = employee.OrganizationalAssignment;
                assignment = new Domain.ValueObjects.OrganizationalAssignment(
                    assignment.UnitBusinessId,
                    assignment.UnitOrgId,
                    assignment.UnitCode,
                    assignment.Unit,
                    request.ToDesignation,
                    assignment.HRRoleId);

                employee.UpdateOrganizationalAssignment(assignment);

                // Add domain event
                var @event = new Domain.Events.EmployeePromotedEvent
                {
                    EmployeeId = employee.Id,
                    FromDesignation = request.FromDesignation,
                    ToDesignation = request.ToDesignation,
                    FromGrade = request.FromGrade,
                    ToGrade = request.ToGrade,
                    PromotionDate = request.PromotionDate
                };

                employee.AddDomainEvent(@event);

                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse
                {
                    Success = true,
                    Message = "Employee promoted successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Error promoting employee: {ex.Message}"
                };
            }
        }
    }

    /// <summary>
    /// Handler for ReactivateEmployeeCommand
    /// </summary>
    public class ReactivateEmployeeCommandHandler : IRequestHandler<ReactivateEmployeeCommand, BaseResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReactivateEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(ReactivateEmployeeCommand request, CancellationToken cancellationToken)
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

                if (!employee.IsTerminated)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Employee is not terminated."
                    };
                }

                employee.Reactivate();

                // Add domain event
                var @event = new Domain.Events.EmployeeReactivatedEvent
                {
                    EmployeeId = employee.Id,
                    ReactivationDate = DateTime.UtcNow
                };

                employee.AddDomainEvent(@event);

                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse
                {
                    Success = true,
                    Message = "Employee reactivated successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Error reactivating employee: {ex.Message}"
                };
            }
        }
    }

    /// <summary>
    /// Handler for TransferEmployeeCommand
    /// </summary>
    public class TransferEmployeeCommandHandler : IRequestHandler<TransferEmployeeCommand, BaseResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransferEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(TransferEmployeeCommand request, CancellationToken cancellationToken)
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

                var assignment = employee.OrganizationalAssignment;
                assignment = new Domain.ValueObjects.OrganizationalAssignment(
                    assignment.UnitBusinessId,
                    request.ToUnitId,
                    assignment.UnitCode,
                    request.ToUnit,
                    assignment.Designation,
                    assignment.HRRoleId);

                employee.UpdateOrganizationalAssignment(assignment);

                // Add domain event
                var @event = new Domain.Events.EmployeeTransferredEvent
                {
                    EmployeeId = employee.Id,
                    FromUnit = request.FromUnit,
                    FromUnitId = request.FromUnitId,
                    ToUnit = request.ToUnit,
                    ToUnitId = request.ToUnitId,
                    TransferDate = request.TransferDate
                };

                employee.AddDomainEvent(@event);

                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse
                {
                    Success = true,
                    Message = "Employee transferred successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Error transferring employee: {ex.Message}"
                };
            }
        }
    }

    /// <summary>
    /// Handler for DeleteEmployeeCommand  
    /// </summary>
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, BaseResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
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

                _unitOfWork.Employees.Delete(employee);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse
                {
                    Success = true,
                    Message = "Employee deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Error deleting employee: {ex.Message}"
                };
            }
        }
    }
}
