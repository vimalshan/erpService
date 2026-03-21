using AutoMapper;
using MediatR;
using EmployeeService.Application.DTOs;
using EmployeeService.Application.Interfaces;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.Application.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _messagePublisher;

    public CreateEmployeeCommandHandler(
        IEmployeeRepository repository,
        IMapper mapper,
        IMessagePublisher messagePublisher)
    {
        _repository = repository;
        _mapper = mapper;
        _messagePublisher = messagePublisher;
    }

    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsAsync(request.EmployeeCode, cancellationToken))
            throw new InvalidOperationException($"Employee with code '{request.EmployeeCode}' already exists.");

        var employee = Employee.Create(
            request.FirstName,
            request.LastName,
            request.EmployeeCode,
            request.HireDate,
            request.JobTitle,
            request.Department,
            request.UserId,
            request.WarehouseId,
            request.Phone,
            request.Email);

        var created = await _repository.AddAsync(employee, cancellationToken);

        await _messagePublisher.PublishAsync(
            "employee",
            "employee.created",
            new { created.EmployeeId, created.EmployeeCode, Event = "EmployeeCreated" },
            cancellationToken);

        return _mapper.Map<EmployeeDto>(created);
    }
}
