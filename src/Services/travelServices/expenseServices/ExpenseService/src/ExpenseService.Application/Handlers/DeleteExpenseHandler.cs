using ExpenseService.Application.Commands;
using ExpenseService.Domain.Interfaces;
using MediatR;

namespace ExpenseService.Application.Handlers;

public class DeleteExpenseHandler : IRequestHandler<DeleteExpenseCommand, bool>
{
    private readonly IExpenseRepository _repository;

    public DeleteExpenseHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.RequestNumber, request.SerialNumber, cancellationToken);
        if (existing == null) return false;

        await _repository.DeleteAsync(request.RequestNumber, request.SerialNumber, cancellationToken);
        return true;
    }
}
