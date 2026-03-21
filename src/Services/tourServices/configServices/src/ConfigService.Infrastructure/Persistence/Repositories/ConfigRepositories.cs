using ConfigService.Domain.Entities;
using ConfigService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConfigService.Infrastructure.Persistence.Repositories;

public class CurrencyRepository(ConfigDbContext context) : EfRepository<Currency, long>(context), ICurrencyRepository { }

public class ExpenseCurrencyRepository(ConfigDbContext context) : EfRepository<ExpenseCurrency, string>(context), IExpenseCurrencyRepository { }

public class ExpenseGroupRepository(ConfigDbContext context) : EfRepository<ExpenseGroup, string>(context), IExpenseGroupRepository
{
    public async Task<ExpenseGroup?> GetWithMappingsAsync(string id, CancellationToken ct = default) =>
        await DbSet.Include(e => e.Mappings).FirstOrDefaultAsync(e => e.Id == id, ct);
}

public class ExpenseTypeRepository(ConfigDbContext context) : EfRepository<ExpenseType, long>(context), IExpenseTypeRepository { }

public class GlobalPayParamRepository(ConfigDbContext context) : EfRepository<GlobalPayParam, string>(context), IGlobalPayParamRepository { }

public class CalendarGstBuMapRepository(ConfigDbContext context) : EfRepository<CalendarGstBuMap, int>(context), ICalendarGstBuMapRepository { }
