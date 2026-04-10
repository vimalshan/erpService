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
    [GraphQLType(typeof(ExpenseDto))]
    public async Task<ExpenseDto?> GetExpenseById([Service] IMediator mediator, decimal expenseId)
    {
        var query = new GetExpenseByIdQuery { ExpenseId = expenseId };
        return await mediator.Send(query);
    }

    [GraphQLType(typeof(List<ExpenseDto>))]
    public async Task<List<ExpenseDto>> GetExpensesByTrip([Service] IMediator mediator, decimal tripId)
    {
        var query = new GetExpensesByTripQuery { TripId = tripId };
        return await mediator.Send(query);
    }

    [GraphQLType(typeof(TripExpenseSummaryDto))]
    public async Task<TripExpenseSummaryDto?> GetTripSummary([Service] IMediator mediator, decimal tripId)
    {
        var query = new GetTripExpenseSummaryQuery { TripId = tripId };
        return await mediator.Send(query);
    }

    [GraphQLType(typeof(List<ExpenseFileDto>))]
    public async Task<List<ExpenseFileDto>> GetExpenseFiles([Service] IMediator mediator, decimal expenseId)
    {
        var query = new GetExpenseFilesQuery { ExpenseId = expenseId };
        return await mediator.Send(query);
    }

    [GraphQLType(typeof(ExpenseStatisticsDto))]
    public async Task<ExpenseStatisticsDto> GetExpenseStatistics(
        [Service] IMediator mediator,
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
        return await mediator.Send(query);
    }
}

/// <summary>
/// GraphQL Mutation type
/// </summary>
public class Mutation
{
    [GraphQLType(typeof(ExpenseDto))]
    public async Task<ExpenseDto> CreateExpense(
        [Service] IMediator mediator,
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

        return await mediator.Send(command);
    }

    [GraphQLType(typeof(ExpenseDto))]
    public async Task<ExpenseDto> UpdateExpense(
        [Service] IMediator mediator,
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

        return await mediator.Send(command);
    }

    [GraphQLType(typeof(bool))]
    public async Task<bool> DeleteExpense([Service] IMediator mediator, decimal expenseId, decimal deletedBy = 0)
    {
        var command = new DeleteExpenseCommand { ExpenseId = expenseId, DeletedBy = deletedBy };
        return await mediator.Send(command);
    }
}
