using Document.Domain.Entities;

namespace Document.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }
    Microsoft.EntityFrameworkCore.DbSet<Signatory> Signatories { get; }
    Microsoft.EntityFrameworkCore.DbSet<AppraisalLetter> AppraisalLetters { get; }
    Microsoft.EntityFrameworkCore.DbSet<AppraisalLetterNew> AppraisalLettersNew { get; }
    Microsoft.EntityFrameworkCore.DbSet<GeneratedLetter> GeneratedLetters { get; }
    Microsoft.EntityFrameworkCore.DbSet<LetterLogHistory> LetterLogHistories { get; }
    Microsoft.EntityFrameworkCore.DbSet<Annexure1> Annexures1 { get; }
    Microsoft.EntityFrameworkCore.DbSet<Annexure2> Annexures2 { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
