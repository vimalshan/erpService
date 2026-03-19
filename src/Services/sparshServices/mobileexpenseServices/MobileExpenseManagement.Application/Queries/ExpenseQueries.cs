namespace MobileExpenseManagement.Application.Queries;

using MediatR;
using MobileExpenseManagement.Application.DTOs;

/// <summary>
/// Query to get an expense by ID
/// </summary>
public class GetExpenseByIdQuery : IRequest<ExpenseDto?>
{
    public decimal ExpenseId { get; set; }
}

/// <summary>
/// Query to get all expenses for a trip
/// </summary>
public class GetExpensesByTripQuery : IRequest<List<ExpenseDto>>
{
    public decimal TripId { get; set; }
}

/// <summary>
/// Query to get paginated expenses for a trip
/// </summary>
public class GetPaginatedExpensesByTripQuery : IRequest<PaginatedExpenseDto>
{
    public decimal TripId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// Query to get trip expense summary
/// </summary>
public class GetTripExpenseSummaryQuery : IRequest<TripExpenseSummaryDto?>
{
    public decimal TripId { get; set; }
}

/// <summary>
/// Query to get expense files
/// </summary>
public class GetExpenseFilesQuery : IRequest<List<ExpenseFileDto>>
{
    public decimal ExpenseId { get; set; }
}

/// <summary>
/// Query to search expenses by date range
/// </summary>
public class SearchExpensesByDateRangeQuery : IRequest<List<ExpenseDto>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? TripId { get; set; }
    public decimal? CategoryId { get; set; }
}

/// <summary>
/// Query to get expenses by category
/// </summary>
public class GetExpensesByCategoryQuery : IRequest<List<ExpenseDto>>
{
    public decimal CategoryId { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}

/// <summary>
/// Query to get expense statistics
/// </summary>
public class GetExpenseStatisticsQuery : IRequest<ExpenseStatisticsDto>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? TripId { get; set; }
    public decimal? EmployeeId { get; set; }
}

/// <summary>
/// DTO for expense statistics
/// </summary>
public class ExpenseStatisticsDto
{
    public decimal TotalExpenses { get; set; }
    public decimal AverageExpense { get; set; }
    public decimal MaxExpense { get; set; }
    public decimal MinExpense { get; set; }
    public int ExpenseCount { get; set; }
    public Dictionary<decimal, decimal> ExpensesByCategory { get; set; } = new();
}
