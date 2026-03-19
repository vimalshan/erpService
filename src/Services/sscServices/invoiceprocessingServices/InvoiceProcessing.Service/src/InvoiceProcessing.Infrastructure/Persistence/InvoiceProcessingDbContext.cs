using InvoiceProcessing.Domain.Common;
using InvoiceProcessing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InvoiceProcessing.Infrastructure.Persistence;

public class InvoiceProcessingDbContext(DbContextOptions<InvoiceProcessingDbContext> options, IMediator mediator)
    : DbContext(options)
{
    public DbSet<DocumentDetail> Documents => Set<DocumentDetail>();
    public DbSet<OracleInvoiceDetail> OracleInvoiceDetails => Set<OracleInvoiceDetail>();
    public DbSet<OraclePaymentDetail> OraclePaymentDetails => Set<OraclePaymentDetail>();
    public DbSet<OracleBankDetail> OracleBankDetails => Set<OracleBankDetail>();
    public DbSet<DocumentPoList> DocumentPoLists => Set<DocumentPoList>();
    public DbSet<DocumentApprovalDetail> DocumentApprovalDetails => Set<DocumentApprovalDetail>();
    public DbSet<DocumentMrcList> DocumentMrcLists => Set<DocumentMrcList>();
    public DbSet<DocumentCostCenter> DocumentCostCenters => Set<DocumentCostCenter>();
    public DbSet<DocumentAttachment> DocumentAttachments => Set<DocumentAttachment>();
    public DbSet<DocumentSscFile> DocumentSscFiles => Set<DocumentSscFile>();
    public DbSet<DocumentApAllocation> DocumentApAllocations => Set<DocumentApAllocation>();
    public DbSet<DocumentCorrespondence> DocumentCorrespondences => Set<DocumentCorrespondence>();
    public DbSet<DocumentCorrespondenceAttachment> CorrespondenceAttachments => Set<DocumentCorrespondenceAttachment>();
    public DbSet<DocumentDefectiveAttachment> DefectiveAttachments => Set<DocumentDefectiveAttachment>();
    public DbSet<DocumentStatus> DocumentStatuses => Set<DocumentStatus>();
    public DbSet<DocumentRescanDetail> DocumentRescanDetails => Set<DocumentRescanDetail>();
    public DbSet<DocumentRevokeDetail> DocumentRevokeDetails => Set<DocumentRevokeDetail>();
    public DbSet<DocumentApprover> DocumentApprovers => Set<DocumentApprover>();
    public DbSet<DocumentCounter> DocumentCounters => Set<DocumentCounter>();
    public DbSet<DocumentDuplicateCheck> DocumentDuplicateChecks => Set<DocumentDuplicateCheck>();
    public DbSet<OracleDueDetail> OracleDueDetails => Set<OracleDueDetail>();
    public DbSet<DocumentReportField> DocumentReportFields => Set<DocumentReportField>();
    public DbSet<DocumentSharePoint> DocumentSharePoints => Set<DocumentSharePoint>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvoiceProcessingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.Entity.DomainEvents).ToList();
        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, ct);
        }

        return result;
    }
}
