using ProblemManagement.Domain.Entities;
using ProblemManagement.Infrastructure.Data;

namespace ProblemManagement.API.GraphQL;

public class ProblemQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProblemMain> GetProblems([Service] ProblemManagementDbContext context) =>
        context.Problems;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProblemSolution> GetSolutions([Service] ProblemManagementDbContext context) =>
        context.ProblemSolutions;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProblemFunction> GetFunctions([Service] ProblemManagementDbContext context) =>
        context.ProblemFunctions;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProblemImpact> GetImpacts([Service] ProblemManagementDbContext context) =>
        context.ProblemImpacts;
}
