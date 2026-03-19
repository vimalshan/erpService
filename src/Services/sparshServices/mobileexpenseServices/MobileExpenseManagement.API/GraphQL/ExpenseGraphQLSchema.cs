using HotChocolate;
using MobileExpenseManagement.Application.DTOs;
using MobileExpenseManagement.Application.Queries;
using MobileExpenseManagement.Application.Commands;
using MediatR;

namespace MobileExpenseManagement.API.GraphQL;

/// <summary>
/// GraphQL Query type
/// </summary>
public class Query
{
    private readonly IMediator _mediator;

    public Query(IMediator mediator)
    {
        _mediator = mediator;
    }

    [GraphQLType(typeof(ExpenseDto))]
    public async Task<ExpenseDto?> GetExpenseById(decimal expenseId)
    {
        var query = new GetExpenseByIdQuery { ExpenseId = expenseId };
        return await _mediator.Send(query);
    }

    [GraphQLType(typeof(List<ExpenseDto>))]
    public async Task<List<ExpenseDto>> GetExpensesByTrip(decimal tripId)
    {
        var query = new GetExpensesByTripQuery { TripId = tripId };
        return await _mediator.Send(query);
    }

    [GraphQLType(typeof(TripExpenseSummaryDto))]
    public async Task<TripExpenseSummaryDto?> GetTripSummary(decimal tripId)
    {
        var query = new GetTripExpenseSummaryQuery { TripId = tripId };
        return await _mediator.Send(query);
    }

    [GraphQLType(typeof(List<ExpenseFileDto>))]
    public async Task<List<ExpenseFileDto>> GetExpenseFiles(decimal expenseId)
    {
        var query = new GetExpenseFilesQuery { ExpenseId = expenseId };
        return await _mediator.Send(query);
    }

    [GraphQLType(typeof(ExpenseStatisticsDto))]
    public async Task<ExpenseStatisticsDto> GetExpenseStatistics(
        DateTime startDate, 
        DateTime endDate, 
        decimal? tripId = null)
    {
        var query = new GetExpenseStatisticsQuery
        {
            StartDate = startDate,
            EndDate = endDate,
            TripId = tripId
        };
        return await _mediator.Send(query);
    }
}

/// <summary>
/// GraphQL Mutation type
/// </summary>
public class Mutation
{
    private readonly IMediator _mediator;

    public Mutation(IMediator mediator)
    {
        _mediator = mediator;
    }

    [GraphQLType(typeof(ExpenseDto))]
    public async Task<ExpenseDto> CreateExpense(
        decimal tripId,
        decimal categoryId,
        DateTime expenseDate,
        string comment,
        decimal amount,
        decimal? currencyId = null,
        decimal enteredBy = 0)
    {
        var command = new CreateExpenseCommand
        {
            TripId = tripId,
            CategoryId = categoryId,
            ExpenseDate = expenseDate,
            Comment = comment,
            Amount = amount,
            CurrencyId = currencyId,
            EnteredBy = enteredBy
        };

        return await _mediator.Send(command);
    }

    [GraphQLType(typeof(ExpenseDto))]
    public async Task<ExpenseDto> UpdateExpense(
        decimal expenseId,
        string comment,
        decimal amount,
        decimal? currencyId = null,
        decimal modifiedBy = 0)
    {
        var command = new UpdateExpenseCommand
        {
            ExpenseId = expenseId,
            Comment = comment,
            Amount = amount,
            CurrencyId = currencyId,
            ModifiedBy = modifiedBy
        };

        return await _mediator.Send(command);
    }

    [GraphQLType(typeof(bool))]
    public async Task<bool> DeleteExpense(decimal expenseId, decimal deletedBy = 0)
    {
        var command = new DeleteExpenseCommand { ExpenseId = expenseId, DeletedBy = deletedBy };
        return await _mediator.Send(command);
    }
}
