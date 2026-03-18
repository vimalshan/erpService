using LoanDefinition.Domain.Entities;
using LoanDefinition.SharedKernel;

namespace LoanDefinition.Domain.Repositories;

public interface ILoanTypeMasterRepository : IRepository<LoanTypeMaster>
{
    Task<IReadOnlyList<LoanTypeMaster>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
}

public interface ILoanMasterRepository : IRepository<LoanMaster>
{
    Task<LoanMaster?> GetWithDetailsAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LoanMaster>> GetByTypeAsync(long loanTypeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LoanMaster>> GetActiveLoansAsync(CancellationToken cancellationToken = default);
}

public interface ILoanSubClassRepository : IRepository<LoanSubClass>
{
    Task<IReadOnlyList<LoanSubClass>> GetByLoanIdAsync(long loanId, CancellationToken cancellationToken = default);
}

public interface ILoanInterestRateRepository : IRepository<LoanInterestRateMaster>
{
    Task<IReadOnlyList<LoanInterestRateMaster>> GetByLoanIdAsync(long loanId, CancellationToken cancellationToken = default);
    Task<LoanInterestRateMaster?> GetEffectiveRateAsync(long loanId, DateTime asOfDate, CancellationToken cancellationToken = default);
}

public interface ILoanLimitRangeRepository : IRepository<LoanLimitRangeMaster>
{
    Task<IReadOnlyList<LoanLimitRangeMaster>> GetByLoanIdAsync(long loanId, CancellationToken cancellationToken = default);
}

public interface ILoanPerquisiteRepository : IRepository<LoanPerquisite>
{
    Task<IReadOnlyList<LoanPerquisite>> GetByClassIdAsync(string classId, CancellationToken cancellationToken = default);
}

public interface ILoanFestivalRepository : IRepository<LoanFestival>
{
    Task<IReadOnlyList<LoanFestival>> GetActiveFestivalsAsync(DateTime asOfDate, CancellationToken cancellationToken = default);
}

public interface ILoanFestivalMapRepository : IRepository<LoanFestivalMap>
{
    Task<IReadOnlyList<LoanFestivalMap>> GetByLoanIdAsync(long loanId, CancellationToken cancellationToken = default);
}

public interface ILoanAccountMasterRepository : IRepository<LoanAccountMaster>
{
    Task<IReadOnlyList<LoanAccountMaster>> GetByLoanTypeAsync(long loanType, CancellationToken cancellationToken = default);
}
