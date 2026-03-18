using MediatR;
using LeaveServices.Application.DTOs;
using LeaveServices.Domain.Entities;
using LeaveServices.Domain.Repositories;
using LeaveServices.Domain.Services;
using LeaveServices.Domain.ValueObjects;

namespace LeaveServices.Application.Features.LeaveEncashments.Commands;

public sealed class ApplyLeaveEncashmentHandler : IRequestHandler<ApplyLeaveEncashmentCommand, LeaveEncashmentDto>
{
    private readonly ILeaveEncashmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ApplyLeaveEncashmentHandler(ILeaveEncashmentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LeaveEncashmentDto> Handle(ApplyLeaveEncashmentCommand command, CancellationToken ct)
    {
        var amount = EncashmentCalculator.Calculate(command.BasicSalary, command.EncashmentDays);

        var encashment = LeaveEncashment.Create(
            command.EmpSysId,
            command.LeaveType,
            command.EncashmentDays,
            amount,
            command.RequestDate,
            command.RequestedBy);

        await _repository.AddAsync(encashment, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToDto(encashment);
    }

    private static LeaveEncashmentDto MapToDto(LeaveEncashment e) =>
        new(e.EncashmentId, e.EmpSysId, e.LeaveType, e.EncashmentDays,
            e.EncashmentAmount, e.RequestDate, e.EncashmentStatus,
            EncashmentStatus.From(e.EncashmentStatus).Description, e.CreatedOn);
}

public sealed class UpdateEncashmentStatusHandler : IRequestHandler<UpdateEncashmentStatusCommand, LeaveEncashmentDto>
{
    private readonly ILeaveEncashmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEncashmentStatusHandler(ILeaveEncashmentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LeaveEncashmentDto> Handle(UpdateEncashmentStatusCommand command, CancellationToken ct)
    {
        var encashment = await _repository.GetByIdAsync(command.EncashmentId, ct)
            ?? throw new KeyNotFoundException($"Encashment {command.EncashmentId} not found.");

        encashment.UpdateStatus(command.NewStatus, command.ModifiedBy);
        await _unitOfWork.SaveChangesAsync(ct);

        return new LeaveEncashmentDto(
            encashment.EncashmentId, encashment.EmpSysId, encashment.LeaveType,
            encashment.EncashmentDays, encashment.EncashmentAmount, encashment.RequestDate,
            encashment.EncashmentStatus,
            EncashmentStatus.From(encashment.EncashmentStatus).Description,
            encashment.CreatedOn);
    }
}
