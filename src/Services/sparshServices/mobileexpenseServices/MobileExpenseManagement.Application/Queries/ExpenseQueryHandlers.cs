namespace MobileExpenseManagement.Application.Queries;

using MediatR;
using MobileExpenseManagement.Application.DTOs;
using MobileExpenseManagement.Application.Common.Interfaces;
using AutoMapper;

/// <summary>
/// Handler for GetExpenseByIdQuery
/// </summary>
public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, ExpenseDto?>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public GetExpenseByIdQueryHandler(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<ExpenseDto?> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken);
        return expense != null ? _mapper.Map<ExpenseDto>(expense) : null;
    }
}

/// <summary>
/// Handler for GetExpensesByTripQuery
/// </summary>
public class GetExpensesByTripQueryHandler : IRequestHandler<GetExpensesByTripQuery, List<ExpenseDto>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public GetExpensesByTripQueryHandler(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<List<ExpenseDto>> Handle(GetExpensesByTripQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _expenseRepository.GetByTripIdAsync(request.TripId, cancellationToken);
        return _mapper.Map<List<ExpenseDto>>(expenses);
    }
}

/// <summary>
/// Handler for GetPaginatedExpensesByTripQuery
/// </summary>
public class GetPaginatedExpensesByTripQueryHandler : IRequestHandler<GetPaginatedExpensesByTripQuery, PaginatedExpenseDto>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public GetPaginatedExpensesByTripQueryHandler(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedExpenseDto> Handle(GetPaginatedExpensesByTripQuery request, CancellationToken cancellationToken)
    {
        var (expenses, totalCount) = await _expenseRepository.GetByTripIdPaginatedAsync(
            request.TripId, request.PageNumber, request.PageSize, cancellationToken);

        return new PaginatedExpenseDto
        {
            Items = _mapper.Map<List<ExpenseDto>>(expenses),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

/// <summary>
/// Handler for GetExpenseFilesQuery
/// </summary>
public class GetExpenseFilesQueryHandler : IRequestHandler<GetExpenseFilesQuery, List<ExpenseFileDto>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public GetExpenseFilesQueryHandler(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<List<ExpenseFileDto>> Handle(GetExpenseFilesQuery request, CancellationToken cancellationToken)
    {
        var files = await _expenseRepository.GetExpenseFilesAsync(request.ExpenseId, cancellationToken);
        return _mapper.Map<List<ExpenseFileDto>>(files);
    }
}

/// <summary>
/// Handler for SearchExpensesByDateRangeQuery
/// </summary>
public class SearchExpensesByDateRangeQueryHandler : IRequestHandler<SearchExpensesByDateRangeQuery, List<ExpenseDto>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public SearchExpensesByDateRangeQueryHandler(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<List<ExpenseDto>> Handle(SearchExpensesByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _expenseRepository.SearchByDateRangeAsync(
            request.StartDate, request.EndDate, request.TripId, request.CategoryId, cancellationToken);

        return _mapper.Map<List<ExpenseDto>>(expenses);
    }
}

/// <summary>
/// Handler for GetTripExpenseSummaryQuery
/// </summary>
public class GetTripExpenseSummaryQueryHandler : IRequestHandler<GetTripExpenseSummaryQuery, TripExpenseSummaryDto?>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public GetTripExpenseSummaryQueryHandler(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<TripExpenseSummaryDto?> Handle(GetTripExpenseSummaryQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _expenseRepository.GetByTripIdAsync(request.TripId, cancellationToken);
        if (expenses.Count == 0)
            return null;

        return new TripExpenseSummaryDto
        {
            TripId = request.TripId,
            TotalExpenseAmount = expenses.Sum(e => e.Amount),
            ExpenseCount = expenses.Count,
            Expenses = _mapper.Map<List<ExpenseDto>>(expenses)
        };
    }
}

/// <summary>
/// Handler for GetExpenseStatisticsQuery
/// </summary>
public class GetExpenseStatisticsQueryHandler : IRequestHandler<GetExpenseStatisticsQuery, ExpenseStatisticsDto>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetExpenseStatisticsQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<ExpenseStatisticsDto> Handle(GetExpenseStatisticsQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _expenseRepository.SearchByDateRangeAsync(
            request.StartDate, request.EndDate, request.TripId, null, cancellationToken);

        if (expenses.Count == 0)
        {
            return new ExpenseStatisticsDto();
        }

        var amounts = expenses.Select(e => e.Amount).ToList();

        return new ExpenseStatisticsDto
        {
            TotalExpenses = amounts.Sum(),
            AverageExpense = amounts.Average(),
            MaxExpense = amounts.Max(),
            MinExpense = amounts.Min(),
            ExpenseCount = expenses.Count,
            ExpensesByCategory = expenses
                .GroupBy(e => e.CategoryId)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount))
        };
    }
}
