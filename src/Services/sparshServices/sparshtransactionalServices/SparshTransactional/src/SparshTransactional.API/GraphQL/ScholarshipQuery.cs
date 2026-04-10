using SparshTransactional.Domain.Entities;
using SparshTransactional.Infrastructure.Data;

namespace SparshTransactional.API.GraphQL;

public class ScholarshipQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ScholarshipMaster> GetScholarships([Service] SparshTransactionalDbContext context) =>
        context.Scholarships;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EligibilityCriteria> GetEligibilityCriteria([Service] SparshTransactionalDbContext context) =>
        context.EligibilityCriteria;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ScholarshipApplication> GetApplications([Service] SparshTransactionalDbContext context) =>
        context.Applications;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ScholarshipDisbursement> GetDisbursements([Service] SparshTransactionalDbContext context) =>
        context.Disbursements;
}
