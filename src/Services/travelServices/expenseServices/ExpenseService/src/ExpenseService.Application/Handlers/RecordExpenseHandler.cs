using AutoMapper;
using ExpenseService.Application.Commands;
using ExpenseService.Application.DTOs;
using ExpenseService.Domain.Entities;
using ExpenseService.Domain.Events;
using ExpenseService.Domain.Interfaces;
using MediatR;

namespace ExpenseService.Application.Handlers;

public class RecordExpenseHandler : IRequestHandler<RecordExpenseCommand, TravelExpenseDto>
{
    private readonly IExpenseRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public RecordExpenseHandler(IExpenseRepository repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<TravelExpenseDto> Handle(RecordExpenseCommand request, CancellationToken cancellationToken)
    {
        var existingExpenses = await _repository.GetByRequestNumberAsync(request.RequestNumber, cancellationToken);
        var nextSerial = existingExpenses.Count > 0 ? existingExpenses.Max(e => e.SerialNumber) + 1 : 1;

        var variance = request.EligibleAmount - request.BudgetAmount;

        var expense = new TravelExpense
        {
            RequestNumber = request.RequestNumber,
            SerialNumber = nextSerial,
            ExpenseCode = request.ExpenseCode,
            CurrencyType = request.CurrencyType,
            BudgetAmount = request.BudgetAmount,
            EligibleAmount = request.EligibleAmount,
            SelfExpense = request.SelfAmount,
            VarianceAmount = variance,
            ExpenseRemarks = request.ExpenseRemarks
        };

        var created = await _repository.AddAsync(expense, cancellationToken);

        await _mediator.Publish(new ExpenseRecordedEvent(
            created.RequestNumber, created.SerialNumber, request.BudgetAmount), cancellationToken);

        return _mapper.Map<TravelExpenseDto>(created);
    }
}
