using MemberService.Domain.Aggregates;
using MemberService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberService.Infrastructure.Data;

public class MemberDbContext : DbContext
{
    public MemberDbContext(DbContextOptions<MemberDbContext> options) : base(options) { }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<MemberNominee> MemberNominees => Set<MemberNominee>();
    public DbSet<MemberPayroll> MemberPayrolls => Set<MemberPayroll>();
    public DbSet<NomineeGuardian> NomineeGuardians => Set<NomineeGuardian>();
    public DbSet<MemberContact> MemberContacts => Set<MemberContact>();
    public DbSet<MemberAuditLog> MemberAuditLogs => Set<MemberAuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MemberDbContext).Assembly);
    }
}
