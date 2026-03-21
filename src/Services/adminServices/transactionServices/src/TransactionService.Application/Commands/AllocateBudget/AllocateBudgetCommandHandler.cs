namespace TransactionService.Application.Commands.AllocateBudget;

using MediatR;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

public sealed class AllocateDeptBudgetCommandHandler : IRequestHandler<AllocateDeptBudgetCommand, bool>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AllocateDeptBudgetCommandHandler(IBudgetRepository budgetRepository, IUnitOfWork unitOfWork)
    {
        _budgetRepository = budgetRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AllocateDeptBudgetCommand request, CancellationToken cancellationToken)
    {
        var existing = await _budgetRepository.GetDeptBudgetAsync(
            request.LocationId, request.DeptId, request.FinYearId, cancellationToken);

        if (existing is not null)
        {
            existing.UpdateBudget(request.BudgetAmount, request.UpdatedBy);
            _budgetRepository.UpdateDeptBudget(existing);
        }
        else
        {
            var budget = DeptBudget.Create(
                request.LocationId, request.UnitCode, request.DeptId,
                request.FinYearId, request.BudgetAmount, request.UpdatedBy);
            await _budgetRepository.AddDeptBudgetAsync(budget, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
        return true;
    }
}

public sealed class AllocateUnitBudgetCommandHandler : IRequestHandler<AllocateUnitBudgetCommand, bool>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AllocateUnitBudgetCommandHandler(IBudgetRepository budgetRepository, IUnitOfWork unitOfWork)
    {
        _budgetRepository = budgetRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AllocateUnitBudgetCommand request, CancellationToken cancellationToken)
    {
        var existing = await _budgetRepository.GetUnitBudgetAsync(
            request.LocationId, request.UnitCode, request.FinYearId, cancellationToken);

        if (existing is not null)
        {
            existing.UpdateBudget(request.BudgetAmount, request.UpdatedBy);
            _budgetRepository.UpdateUnitBudget(existing);
        }
        else
        {
            var budget = UnitBudget.Create(
                request.LocationId, request.UnitCode, request.FinYearId,
                request.BudgetAmount, request.UpdatedBy);
            await _budgetRepository.AddUnitBudgetAsync(budget, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
        return true;
    }
}
