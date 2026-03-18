using ExitManagement.Domain.Entities;

namespace ExitManagement.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    Microsoft.EntityFrameworkCore.DbSet<EmployeeExit> EmployeeExits { get; }
    Microsoft.EntityFrameworkCore.DbSet<ExitInterviewFeedback> ExitInterviewFeedbacks { get; }
    Microsoft.EntityFrameworkCore.DbSet<ExitQuestion> ExitQuestions { get; }
    Microsoft.EntityFrameworkCore.DbSet<ExitInterviewQuestion> ExitInterviewQuestions { get; }
    Microsoft.EntityFrameworkCore.DbSet<ExitResponsibilityMap> ExitResponsibilityMaps { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
