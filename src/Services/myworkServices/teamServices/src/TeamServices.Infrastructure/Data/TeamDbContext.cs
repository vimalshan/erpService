using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamServices.Domain.Common;
using TeamServices.Domain.Entities;
using TeamServices.Domain.Interfaces;

namespace TeamServices.Infrastructure.Data;

public class TeamDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public DbSet<TeamMaster> Teams => Set<TeamMaster>();
    public DbSet<TeamEmployeeMap> TeamEmployeeMaps => Set<TeamEmployeeMap>();
    public DbSet<TeamUnitMap> TeamUnitMaps => Set<TeamUnitMap>();

    public TeamDbContext(DbContextOptions<TeamDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TEAM_MASTER
        modelBuilder.Entity<TeamMaster>(entity =>
        {
            entity.ToTable("TEAM_MASTER");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("TEAM_ID").ValueGeneratedNever();
            entity.Property(e => e.TeamName).HasColumnName("TEAM_NAME").HasMaxLength(50).IsRequired();
            entity.Property(e => e.LastModifiedBy).HasColumnName("TEAM_LASTMODIFIEDBY");
            entity.Property(e => e.LastModifiedOn).HasColumnName("TEAM_LASTMODIFIEDON").HasColumnType("datetime2(3)");

            entity.HasMany(e => e.EmployeeMaps)
                .WithOne(e => e.Team)
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.UnitMaps)
                .WithOne(e => e.Team)
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // TEAM_EMPMAP
        modelBuilder.Entity<TeamEmployeeMap>(entity =>
        {
            entity.ToTable("TEAM_EMPMAP");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("TEAMEMP_ID").ValueGeneratedNever();
            entity.Property(e => e.TeamId).HasColumnName("TEAMEMP_TEAMID");
            entity.Property(e => e.EmployeeSysId).HasColumnName("TEAMEMP_EMPSYSID");
            entity.Property(e => e.EffectiveDate).HasColumnName("TEAMEMP_EFFDATE").HasColumnType("datetime2(3)");
            entity.Property(e => e.CloseDate).HasColumnName("TEAMEMP_CLOSEDATE").HasColumnType("datetime2(3)");
            entity.Property(e => e.LastModifiedBy).HasColumnName("TEAMEMP_LASTMODIFIEDBY");
            entity.Property(e => e.LastModifiedOn).HasColumnName("TEAMEMP_LASTMODIFIEDON").HasColumnType("datetime2(3)");
        });

        // TEAM_UNITMAP
        modelBuilder.Entity<TeamUnitMap>(entity =>
        {
            entity.ToTable("TEAM_UNITMAP");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("TEAM_MAPID").ValueGeneratedNever();
            entity.Property(e => e.TeamId).HasColumnName("TEAM_ID");
            entity.Property(e => e.UnitId).HasColumnName("TEAM_UNITID");
            entity.Property(e => e.GradeCategory).HasColumnName("TEAM_GRADECATEGORY").HasColumnType("char(1)");
            entity.Property(e => e.CadreId).HasColumnName("TEAM_CADREID");
            entity.Property(e => e.LastModifiedBy).HasColumnName("TEAM_LASTMODIFIEDBY");
            entity.Property(e => e.LastModifiedOn).HasColumnName("TEAM_LASTMODIFIEDON").HasColumnType("datetime2(3)");
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
