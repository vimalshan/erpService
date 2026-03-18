using EmployeeManagement.Application.Transfers.DTOs;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace EmployeeManagement.Application.Transfers.Commands.CreateTransfer;

public sealed record CreateTransferCommand(
    long TransferId,
    long EmployeeId,
    string OldUnit,
    string NewUnit,
    long OldUnitId,
    long NewUnitId,
    long? ReasonId,
    DateTime TransferDate,
    string? Remarks,
    string TransferType,
    bool PayrollTransfer,
    long CreatedBy
) : IRequest<TransferDto>;

public sealed class CreateTransferCommandValidator : AbstractValidator<CreateTransferCommand>
{
    public CreateTransferCommandValidator()
    {
        RuleFor(x => x.EmployeeId).GreaterThan(0);
        RuleFor(x => x.NewUnit).NotEmpty().MaximumLength(3);
        RuleFor(x => x.TransferDate).NotEmpty();
        RuleFor(x => x.TransferType).NotEmpty().MaximumLength(2);
    }
}

public sealed class CreateTransferCommandHandler : IRequestHandler<CreateTransferCommand, TransferDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransferCommandHandler(IEmployeeRepository employeeRepository,
        ITransferRepository transferRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _transferRepository = transferRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransferDto> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new EmployeeNotFoundException(request.EmployeeId);

        var transfer = EmployeeTransfer.Create(
            request.TransferId, request.EmployeeId, request.OldUnit, request.NewUnit,
            request.OldUnitId, request.NewUnitId, request.ReasonId, request.TransferDate,
            request.Remarks, request.TransferType, request.PayrollTransfer, request.CreatedBy);

        employee.Transfer(request.NewUnit, request.NewUnitId, request.TransferId, request.CreatedBy);
        _employeeRepository.Update(employee);
        await _transferRepository.AddAsync(transfer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TransferDto(transfer.TransferId, transfer.EmployeeId, transfer.OldUnit, transfer.NewUnit,
            transfer.OldUnitId, transfer.NewUnitId, transfer.TransferDate, transfer.Remarks,
            transfer.TransferType, transfer.Status, transfer.CreatedBy, transfer.CreatedOn);
    }
}
