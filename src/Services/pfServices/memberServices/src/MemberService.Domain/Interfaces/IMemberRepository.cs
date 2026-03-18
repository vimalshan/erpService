using MemberService.Domain.Aggregates;

namespace MemberService.Domain.Interfaces;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(long memberNo, CancellationToken ct = default);
    Task<Member?> GetByEmployeeSysIdAsync(long employeeSysId, CancellationToken ct = default);
    Task<IReadOnlyList<Member>> GetAllActiveAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Member>> GetByTrustCodeAsync(string trustCode, CancellationToken ct = default);
    Task<bool> ExistsByEmployeeSysIdAsync(long employeeSysId, CancellationToken ct = default);
    Task<long> GetNextMemberNumberAsync(CancellationToken ct = default);
    Task AddAsync(Member member, CancellationToken ct = default);
    Task UpdateAsync(Member member, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
