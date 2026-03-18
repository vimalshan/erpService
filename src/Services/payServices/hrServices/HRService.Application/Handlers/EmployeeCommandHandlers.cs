using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using HRService.Application.Commands;
using HRService.Application.DTOs;
using HRService.Infrastructure.Repositories;
using HRService.Domain.Entities;

namespace HRService.Application.Handlers;

/// <summary>
/// Command handlers for employee operations
/// </summary>
public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateEmployeeCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating employee with code: {EmployeeCode}", request.EmployeeCode);

        try
        {
            // Validate department exists
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(request.DepartmentId, cancellationToken);
            if (department == null)
                throw new InvalidOperationException($"Department {request.DepartmentId} not found");

            // Validate position exists
            var position = await _unitOfWork.PositionRepository.GetByIdAsync(request.PositionId, cancellationToken);
            if (position == null)
                throw new InvalidOperationException($"Position {request.PositionId} not found");

            // Create employee
            var employee = Employee.Create(
                request.EmployeeCode,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.Email,
                request.DepartmentId,
                request.PositionId,
                request.SiteId,
                request.JoinDate,
                Enum.Parse<EmploymentType>(request.EmploymentType),
                request.MiddleName,
                request.Gender,
                request.PhoneNumber,
                null,
                request.ManagerId
            );

            await _unitOfWork.EmployeeRepository.AddAsync(employee, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Employee created successfully with ID: {EmployeeId}", employee.Id);
            return employee.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            throw;
        }
    }
}

public class TerminateEmployeeCommandHandler : IRequestHandler<TerminateEmployeeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TerminateEmployeeCommandHandler> _logger;

    public TerminateEmployeeCommandHandler(IUnitOfWork unitOfWork, ILogger<TerminateEmployeeCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(TerminateEmployeeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Terminating employee: {EmployeeId}", request.EmployeeId);

        try
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
            if (employee == null)
                throw new InvalidOperationException($"Employee {request.EmployeeId} not found");

            employee.Terminate(request.TerminationDate, request.Reason);

            await _unitOfWork.EmployeeRepository.UpdateAsync(employee, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Employee {EmployeeId} terminated successfully", request.EmployeeId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error terminating employee");
            return false;
        }
    }
}

public class UpdateEmployeePositionCommandHandler : IRequestHandler<UpdateEmployeePositionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateEmployeePositionCommandHandler> _logger;

    public UpdateEmployeePositionCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateEmployeePositionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateEmployeePositionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
            if (employee == null)
                throw new InvalidOperationException($"Employee {request.EmployeeId} not found");

            employee.UpdatePosition(request.PositionId);

            await _unitOfWork.EmployeeRepository.UpdateAsync(employee, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Employee {EmployeeId} position updated", request.EmployeeId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee position");
            return false;
        }
    }
}

public class SuspendEmployeeCommandHandler : IRequestHandler<SuspendEmployeeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public SuspendEmployeeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(SuspendEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee == null)
            return false;

        employee.Suspend();
        await _unitOfWork.EmployeeRepository.UpdateAsync(employee, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class ResumeEmployeeCommandHandler : IRequestHandler<ResumeEmployeeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public ResumeEmployeeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ResumeEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee == null)
            return false;

        employee.Resume();
        await _unitOfWork.EmployeeRepository.UpdateAsync(employee, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
