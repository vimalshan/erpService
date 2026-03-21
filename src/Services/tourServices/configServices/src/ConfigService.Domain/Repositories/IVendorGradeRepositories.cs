using ConfigService.Domain.Common;
using ConfigService.Domain.Entities;

namespace ConfigService.Domain.Repositories;

public interface IVendorRepository : IRepository<Vendor, string>
{
    Task<Vendor?> GetWithDetailsAsync(string id, CancellationToken ct = default);
    Task<IReadOnlyList<Vendor>> GetActiveVendorsAsync(CancellationToken ct = default);
}

public interface IGradeCatExpenseRuleRepository : IRepository<GradeCatExpenseRule, string>
{
    Task<GradeCatExpenseRule?> GetWithBreaksAsync(string id, CancellationToken ct = default);
    Task<IReadOnlyList<GradeCatExpenseRule>> GetByGradeCategoryAsync(string gradeCategory, CancellationToken ct = default);
}

public interface IGradeCatModeMapRepository : IRepository<GradeCatModeMap, string> { }

public interface IGradeCatStayRuleRepository : IRepository<GradeCatStayRule, string> { }

public interface IGradeCatExpenseMapRepository : IRepository<GradeCatExpenseMap, string> { }

public interface IGradeTypeTravelParamRepository : IRepository<GradeTypeTravelParam, string> { }
