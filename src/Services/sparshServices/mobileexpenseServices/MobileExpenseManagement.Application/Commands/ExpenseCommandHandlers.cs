namespace MobileExpenseManagement.Application.Commands;

using MediatR;
using MobileExpenseManagement.Application.DTOs;
using MobileExpenseManagement.Application.Common.Interfaces;
using MobileExpenseManagement.Domain.Entities;
using FluentValidation;
using AutoMapper;
using Microsoft.Extensions.Logging;

/// <summary>
/// Validator for CreateExpenseCommand
/// </summary>
public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator()
    {
        RuleFor(x => x.TripId).GreaterThan(0).WithMessage("TripId must be greater than 0");
        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("CategoryId must be greater than 0");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0");
        RuleFor(x => x.Comment).NotEmpty().MinimumLength(5).WithMessage("Comment must be at least 5 characters");
        RuleFor(x => x.EnteredBy).GreaterThan(0).WithMessage("EnteredBy must be greater than 0");
    }
}

/// <summary>
/// Handler for CreateExpenseCommand
/// </summary>
public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, ExpenseDto>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;
    private readonly ILogger<CreateExpenseCommandHandler> _logger;

    public CreateExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPublisher publisher,
        ILogger<CreateExpenseCommandHandler> logger)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<ExpenseDto> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = Expense.Create(
            request.TripId,
            request.CategoryId,
            request.ExpenseDate,
            request.Comment,
            request.Amount,
            request.CurrencyId,
            request.EnteredBy);

        await _expenseRepository.AddAsync(expense, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Publish domain events
        foreach (var domainEvent in expense.DomainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        expense.ClearDomainEvents();

        _logger.LogInformation($"Expense created with ID: {expense.Id}");

        return _mapper.Map<ExpenseDto>(expense);
    }
}

/// <summary>
/// Validator for UpdateExpenseCommand
/// </summary>
public class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator()
    {
        RuleFor(x => x.ExpenseId).GreaterThan(0).WithMessage("ExpenseId must be greater than 0");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0");
        RuleFor(x => x.Comment).NotEmpty().MinimumLength(5).WithMessage("Comment must be at least 5 characters");
        RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage("ModifiedBy must be greater than 0");
    }
}

/// <summary>
/// Handler for UpdateExpenseCommand
/// </summary>
public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, ExpenseDto>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;
    private readonly ILogger<UpdateExpenseCommandHandler> _logger;

    public UpdateExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPublisher publisher,
        ILogger<UpdateExpenseCommandHandler> logger)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<ExpenseDto> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken);
        if (expense == null)
            throw new InvalidOperationException($"Expense with ID {request.ExpenseId} not found");

        expense.Update(request.Comment, request.Amount, request.CurrencyId, request.ModifiedBy);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Publish domain events
        foreach (var domainEvent in expense.DomainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        expense.ClearDomainEvents();

        _logger.LogInformation($"Expense updated: {request.ExpenseId}");

        return _mapper.Map<ExpenseDto>(expense);
    }
}

/// <summary>
/// Handler for DeleteExpenseCommand
/// </summary>
public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, bool>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;
    private readonly ILogger<DeleteExpenseCommandHandler> _logger;

    public DeleteExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork,
        IPublisher publisher,
        ILogger<DeleteExpenseCommandHandler> logger)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken);
        if (expense == null)
            throw new InvalidOperationException($"Expense with ID {request.ExpenseId} not found");

        expense.Delete(request.DeletedBy);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Publish domain events
        foreach (var domainEvent in expense.DomainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        expense.ClearDomainEvents();

        _logger.LogInformation($"Expense deleted: {request.ExpenseId}");

        return true;
    }
}
