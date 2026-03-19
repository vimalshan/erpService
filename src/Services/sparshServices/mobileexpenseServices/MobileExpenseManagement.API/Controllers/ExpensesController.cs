namespace MobileExpenseManagement.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileExpenseManagement.Application.Commands;
using MobileExpenseManagement.Application.DTOs;
using MobileExpenseManagement.Application.Queries;

/// <summary>
/// REST API controller for expense management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ExpensesController> _logger;

    public ExpensesController(IMediator mediator, ILogger<ExpensesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get expense by ID
    /// </summary>
    [HttpGet("{expenseId}")]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExpenseDto>> GetExpenseById(decimal expenseId)
    {
        var query = new GetExpenseByIdQuery { ExpenseId = expenseId };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound($"Expense with ID {expenseId} not found");

        return Ok(result);
    }

    /// <summary>
    /// Get expenses for a trip
    /// </summary>
    [HttpGet("trip/{tripId}")]
    [ProducesResponseType(typeof(List<ExpenseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ExpenseDto>>> GetExpensesByTrip(decimal tripId)
    {
        var query = new GetExpensesByTripQuery { TripId = tripId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get paginated expenses for a trip
    /// </summary>
    [HttpGet("trip/{tripId}/paginated")]
    [ProducesResponseType(typeof(PaginatedExpenseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedExpenseDto>> GetPaginatedExpensesByTrip(
        decimal tripId, 
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = new GetPaginatedExpensesByTripQuery 
        { 
            TripId = tripId, 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Create a new expense
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ExpenseDto>> CreateExpense(
        [FromBody] CreateExpenseDto createExpenseDto,
        [FromHeader(Name = "X-User-Id")] decimal userId)
    {
        var command = new CreateExpenseCommand
        {
            TripId = createExpenseDto.TripId,
            CategoryId = createExpenseDto.CategoryId,
            ExpenseDate = createExpenseDto.ExpenseDate,
            Comment = createExpenseDto.Comment,
            Amount = createExpenseDto.Amount,
            CurrencyId = createExpenseDto.CurrencyId,
            EnteredBy = userId
        };

        var result = await _mediator.Send(command);
        _logger.LogInformation($"Expense created: {result.Id}");
        return CreatedAtAction(nameof(GetExpenseById), new { expenseId = result.Id }, result);
    }

    /// <summary>
    /// Update an existing expense
    /// </summary>
    [HttpPut("{expenseId}")]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExpenseDto>> UpdateExpense(
        decimal expenseId,
        [FromBody] UpdateExpenseDto updateExpenseDto,
        [FromHeader(Name = "X-User-Id")] decimal userId)
    {
        var command = new UpdateExpenseCommand
        {
            ExpenseId = expenseId,
            Comment = updateExpenseDto.Comment,
            Amount = updateExpenseDto.Amount,
            CurrencyId = updateExpenseDto.CurrencyId,
            ModifiedBy = userId
        };

        var result = await _mediator.Send(command);
        _logger.LogInformation($"Expense updated: {expenseId}");
        return Ok(result);
    }

    /// <summary>
    /// Delete an expense
    /// </summary>
    [HttpDelete("{expenseId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExpense(
        decimal expenseId,
        [FromHeader(Name = "X-User-Id")] decimal userId)
    {
        var command = new DeleteExpenseCommand { ExpenseId = expenseId, DeletedBy = userId };
        await _mediator.Send(command);
        _logger.LogInformation($"Expense deleted: {expenseId}");
        return NoContent();
    }

    /// <summary>
    /// Search expenses by date range
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<ExpenseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ExpenseDto>>> SearchExpenses(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] decimal? tripId = null,
        [FromQuery] decimal? categoryId = null)
    {
        var query = new SearchExpensesByDateRangeQuery
        {
            StartDate = startDate,
            EndDate = endDate,
            TripId = tripId,
            CategoryId = categoryId
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get expense statistics
    /// </summary>
    [HttpGet("statistics")]
    [ProducesResponseType(typeof(ExpenseStatisticsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ExpenseStatisticsDto>> GetStatistics(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] decimal? tripId = null,
        [FromQuery] decimal? employeeId = null)
    {
        var query = new GetExpenseStatisticsQuery
        {
            StartDate = startDate,
            EndDate = endDate,
            TripId = tripId,
            EmployeeId = employeeId
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get trip expense summary
    /// </summary>
    [HttpGet("trip/{tripId}/summary")]
    [ProducesResponseType(typeof(TripExpenseSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TripExpenseSummaryDto>> GetTripSummary(decimal tripId)
    {
        var query = new GetTripExpenseSummaryQuery { TripId = tripId };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound($"Trip with ID {tripId} not found");

        return Ok(result);
    }
}
