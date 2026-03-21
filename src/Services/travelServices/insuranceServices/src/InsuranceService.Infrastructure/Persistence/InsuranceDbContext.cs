using InsuranceService.Domain.Common;
using InsuranceService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InsuranceService.Infrastructure.Persistence;

public class InsuranceDbContext : DbContext
{
    private readonly IMediator _mediator;

    public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<TravelInsurance> TravelInsurances => Set<TravelInsurance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TravelInsurance>(entity =>
        {
            entity.ToTable("TRAVEL_INSURANCE");
            entity.HasKey(e => new { e.CompanyCode, e.PlanNumber });

            entity.Property(e => e.CompanyCode)
                .HasColumnName("IN_COM_COD")
                .HasMaxLength(3)
                .IsFixedLength()
                .HasConversion(v => v.Value, v => new Domain.ValueObjects.CompanyCode(v));

            entity.Property(e => e.PlanNumber)
                .HasColumnName("IN_PLN_NUM");

            entity.Property(e => e.InsuranceType)
                .HasColumnName("IN_INS_TYP")
                .HasMaxLength(3)
                .IsFixedLength()
                .HasConversion(v => v.Value, v => new Domain.ValueObjects.InsuranceType(v));

            entity.Property(e => e.PassportNumber)
                .HasColumnName("IN_PASS_NUM")
                .HasMaxLength(50);

            entity.Property(e => e.PassportIssueDate)
                .HasColumnName("IN_ISS_DAT");

            entity.Property(e => e.VisaIssuePlace)
                .HasColumnName("IN_VIS_PLC")
                .HasMaxLength(50);

            entity.Property(e => e.VisaIssueDate)
                .HasColumnName("IN_VIS_DAT");

            entity.Property(e => e.NomineeName1)
                .HasColumnName("IN_NOM_NAM1")
                .HasMaxLength(200);

            entity.Property(e => e.NomineeName2)
                .HasColumnName("IN_NOM_NAM2")
                .HasMaxLength(200);

            entity.Property(e => e.Status)
                .HasColumnName("IN_INS_STS")
                .HasMaxLength(1)
                .IsFixedLength()
                .HasConversion(v => v.Value, v => new Domain.ValueObjects.InsuranceStatus(v));

            entity.Property(e => e.CertificateNumber)
                .HasColumnName("IN_CRT_NUM")
                .HasMaxLength(200);

            entity.Property(e => e.UpdateDate)
                .HasColumnName("IN_UPD_DAT");

            entity.Property(e => e.UpdatedByUserId)
                .HasColumnName("IN_UPD_UID")
                .HasMaxLength(200);

            entity.Property(e => e.UpdatedByUserNumber)
                .HasColumnName("IN_UPD_UNUM");

            entity.Property(e => e.Remarks)
                .HasColumnName("IN_REM_MRK")
                .HasMaxLength(200);

            entity.Property(e => e.FlexField1)
                .HasColumnName("IN_FLX_FLD1")
                .HasMaxLength(200);

            entity.Property(e => e.FlexField2)
                .HasColumnName("IN_FLX_FLD2")
                .HasColumnType("decimal(19,0)");

            entity.Property(e => e.FlexField3)
                .HasColumnName("IN_FLX_FLD3")
                .HasColumnType("decimal(19,0)");

            entity.Property(e => e.FlexField4)
                .HasColumnName("IN_FLX_FLD4");

            entity.Ignore(e => e.DomainEvents);
            entity.Ignore(e => e.Version);
        });

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
