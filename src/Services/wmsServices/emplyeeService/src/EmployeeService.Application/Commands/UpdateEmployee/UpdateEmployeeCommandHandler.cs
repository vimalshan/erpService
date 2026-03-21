using AutoMapper;
using MediatR;
using EmployeeService.Application.DTOs;
using EmployeeService.Application.Interfaces;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.Application.Commands.UpdateEmployee;

public sealed class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _messagePublisher;

    public UpdateEmployeeCommandHandler(
        IEmployeeRepository repository,
        IMapper mapper,
        IMessagePublisher messagePublisher)
    {
        _repository = repository;
        _mapper = mapper;
        _messagePublisher = messagePublisher;
    }

    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee with ID {request.EmployeeId} not found.");

        employee.Update(
            request.FirstName,
            request.LastName,
            request.HireDate,
            request.JobTitle,
            request.Department,
            request.UserId,
            request.WarehouseId,
            request.Phone,
            request.Email);

        await _repository.UpdateAsync(employee, cancellationToken);

        await _messagePublisher.PublishAsync(
            "employee",
            "employee.updated",
            new { employee.EmployeeId, Event = "EmployeeUpdated" },
            cancellationToken);

        return _mapper.Map<EmployeeDto>(employee);
    }
}
