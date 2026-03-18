using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeService.Application.Commands;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.Repositories;
using EmployeeService.Domain.ValueObjects;
using MediatR;

namespace EmployeeService.Application.Handlers.Commands;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // Check if employee already exists
        if (await _employeeRepository.ExistsAsync(request.EmployeeSystemId, cancellationToken))
            throw new InvalidOperationException($"Employee with System ID {request.EmployeeSystemId} already exists");

        var employee = new Employee(
            request.EmployeeSystemId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.EmployeeCode,
            request.JoiningDate)
        {
            MiddleName = request.MiddleName,
            PhoneNumber = request.PhoneNumber,
            CostCenterId = request.CostCenterId
        };

        // Initialize CTC
        employee.InitializeCTC(
            new Money(request.GrossCTC),
            new Money(request.BasicSalary),
            request.CTCEffectiveDate);

        var createdEmployee = await _employeeRepository.AddAsync(employee, cancellationToken);
        return _mapper.Map<EmployeeDto>(createdEmployee);
    }
}

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetBySystemIdAsync(request.EmployeeSystemId, cancellationToken);
        if (employee == null)
            throw new InvalidOperationException($"Employee with System ID {request.EmployeeSystemId} not found");

        employee.UpdatePersonalInformation(
            request.FirstName,
            request.LastName,
            request.MiddleName,
            request.Email,
            request.PhoneNumber);

        if (!string.IsNullOrWhiteSpace(request.CostCenterId))
        {
            employee.AssignCostCenter(request.CostCenterId);
        }

        var updatedEmployee = await _employeeRepository.UpdateAsync(employee, cancellationToken);
        return _mapper.Map<EmployeeDto>(updatedEmployee);
    }
}

public class ProcessSalaryIncrementCommandHandler : IRequestHandler<ProcessSalaryIncrementCommand, SalaryIncrementLogDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ISalaryIncrementLogRepository _logRepository;
    private readonly IMapper _mapper;

    public ProcessSalaryIncrementCommandHandler(
        IEmployeeRepository employeeRepository,
        ISalaryIncrementLogRepository logRepository,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _logRepository = logRepository;
        _mapper = mapper;
    }

    public async Task<SalaryIncrementLogDto> Handle(ProcessSalaryIncrementCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetBySystemIdAsync(request.EmployeeSystemId, cancellationToken);
        if (employee == null)
            throw new InvalidOperationException($"Employee with System ID {request.EmployeeSystemId} not found");

        if (!employee.CanReceiveIncrement())
            throw new InvalidOperationException("Employee is not eligible for increment");

        var incrementPercentage = new Percentage(request.IncrementPercentage);
        
        // Process the increment on the employee
        employee.IncrementCTC(incrementPercentage, request.EffectiveDate, request.ApprovedBy);
        
        // Update employee
        await _employeeRepository.UpdateAsync(employee, cancellationToken);

        // Create log entry
        var log = new SalaryIncrementLog(
            request.EmployeeSystemId,
            employee.GrossCTC,
            employee.GrossCTC,
            incrementPercentage,
            request.EffectiveDate,
            request.ApprovedBy);

        var createdLog = await _logRepository.AddAsync(log, cancellationToken);
        return _mapper.Map<SalaryIncrementLogDto>(createdLog);
    }
}

public class ModifyEmployeeCTCCommandHandler : IRequestHandler<ModifyEmployeeCTCCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public ModifyEmployeeCTCCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(ModifyEmployeeCTCCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetBySystemIdAsync(request.EmployeeSystemId, cancellationToken);
        if (employee == null)
            throw new InvalidOperationException($"Employee with System ID {request.EmployeeSystemId} not found");

        employee.ModifyCTC(
            new Money(request.NewGrossCTC),
            new Money(request.NewBasicSalary),
            request.EffectiveDate,
            request.Reason,
            request.ModifiedBy);

        var updatedEmployee = await _employeeRepository.UpdateAsync(employee, cancellationToken);
        return _mapper.Map<EmployeeDto>(updatedEmployee);
    }
}

public class TerminateEmployeeCommandHandler : IRequestHandler<TerminateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public TerminateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(TerminateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetBySystemIdAsync(request.EmployeeSystemId, cancellationToken);
        if (employee == null)
            throw new InvalidOperationException($"Employee with System ID {request.EmployeeSystemId} not found");

        employee.Terminate(request.TerminationDate);

        var updatedEmployee = await _employeeRepository.UpdateAsync(employee, cancellationToken);
        return _mapper.Map<EmployeeDto>(updatedEmployee);
    }
}

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly IEmployeeRepository _employeeRepository;

    public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetBySystemIdAsync(request.EmployeeSystemId, cancellationToken);
        if (employee == null)
            throw new InvalidOperationException($"Employee with System ID {request.EmployeeSystemId} not found");

        return await _employeeRepository.DeleteAsync(employee.Id, cancellationToken);
    }
}
