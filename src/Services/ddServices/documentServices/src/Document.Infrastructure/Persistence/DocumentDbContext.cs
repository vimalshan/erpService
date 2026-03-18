using Microsoft.EntityFrameworkCore;
using Document.Application.Common.Interfaces;
using Document.Domain.Entities;

namespace Document.Infrastructure.Persistence;

public class DocumentDbContext : DbContext, IApplicationDbContext
{
    public DocumentDbContext(DbContextOptions<DocumentDbContext> options) : base(options) { }

    public DbSet<Signatory> Signatories => Set<Signatory>();
    public DbSet<AppraisalLetter> AppraisalLetters => Set<AppraisalLetter>();
    public DbSet<AppraisalLetterNew> AppraisalLettersNew => Set<AppraisalLetterNew>();
    public DbSet<GeneratedLetter> GeneratedLetters => Set<GeneratedLetter>();
    public DbSet<LetterLogHistory> LetterLogHistories => Set<LetterLogHistory>();
    public DbSet<Annexure1> Annexures1 => Set<Annexure1>();
    public DbSet<Annexure2> Annexures2 => Set<Annexure2>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
