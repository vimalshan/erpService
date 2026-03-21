using ConfigService.Domain.Common;
using ConfigService.Domain.Entities;

namespace ConfigService.Domain.Repositories;

public interface ICurrencyRepository : IRepository<Currency, long> { }

public interface IExpenseCurrencyRepository : IRepository<ExpenseCurrency, string> { }

public interface IExpenseGroupRepository : IRepository<ExpenseGroup, string>
{
    Task<ExpenseGroup?> GetWithMappingsAsync(string id, CancellationToken ct = default);
}

public interface IExpenseTypeRepository : IRepository<ExpenseType, long> { }

public interface IGlobalPayParamRepository : IRepository<GlobalPayParam, string> { }

public interface ICalendarGstBuMapRepository : IRepository<CalendarGstBuMap, int> { }
