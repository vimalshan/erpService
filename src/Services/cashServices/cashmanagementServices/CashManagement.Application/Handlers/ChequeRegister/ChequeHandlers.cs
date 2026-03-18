using MediatR;
using CashManagement.Application.Commands.ChequeRegister;
using CashManagement.Application.DTOs;
using CashManagement.Domain.Exceptions;
using CashManagement.Domain.Interfaces;
using CashManagement.Domain.Interfaces.Repositories;

namespace CashManagement.Application.Handlers.ChequeRegister;

public class IssueChequeHandler : IRequestHandler<IssueChequeCommand, ChequeDto>
{
    private readonly IChequeRegisterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public IssueChequeHandler(IChequeRegisterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ChequeDto> Handle(IssueChequeCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.BankAccountId, request.ChequeNumber, cancellationToken);
        if (exists) throw new DuplicateChequeException(request.ChequeNumber);

        var cheque = Domain.Entities.ChequeRegister.Issue(
            request.BankAccountId, request.ChequeNumber, request.PayeeName,
            request.Amount, request.ChequeDate, request.Reference, request.IssuedBy);

        await _repository.AddAsync(cheque, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(cheque);
    }

    private static ChequeDto MapToDto(Domain.Entities.ChequeRegister c)
        => new(c.Id, c.BankAccountId, c.ChequeNumber, c.PayeeName, c.ChequeAmount,
               c.IssueDate, c.ChequeDate, c.Reference, c.Status.ToString(),
               c.BounceReason, c.CreatedOn);
}

public class MarkChequeBouncedHandler : IRequestHandler<MarkChequeBouncedCommand, bool>
{
    private readonly IChequeRegisterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkChequeBouncedHandler(IChequeRegisterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(MarkChequeBouncedCommand request, CancellationToken cancellationToken)
    {
        var cheque = await _repository.GetByIdAsync(request.ChequeId, cancellationToken);
        if (cheque is null) return false;

        cheque.MarkBounced(request.BounceReason, request.ProcessedBy);
        await _repository.UpdateAsync(cheque, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class MarkChequeClearedHandler : IRequestHandler<MarkChequeClearedCommand, bool>
{
    private readonly IChequeRegisterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkChequeClearedHandler(IChequeRegisterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(MarkChequeClearedCommand request, CancellationToken cancellationToken)
    {
        var cheque = await _repository.GetByIdAsync(request.ChequeId, cancellationToken);
        if (cheque is null) return false;

        cheque.MarkCleared(request.ProcessedBy);
        await _repository.UpdateAsync(cheque, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class CancelChequeHandler : IRequestHandler<CancelChequeCommand, bool>
{
    private readonly IChequeRegisterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelChequeHandler(IChequeRegisterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CancelChequeCommand request, CancellationToken cancellationToken)
    {
        var cheque = await _repository.GetByIdAsync(request.ChequeId, cancellationToken);
        if (cheque is null) return false;

        cheque.Cancel(request.ProcessedBy);
        await _repository.UpdateAsync(cheque, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
