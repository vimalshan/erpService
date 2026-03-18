using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkOrderService.Domain.Common;
using WorkOrderService.Domain.Entities;

namespace WorkOrderService.Infrastructure.Persistence;

public class WorkOrderDbContext : DbContext
{
    private readonly IMediator _mediator;

    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<WorkTask> WorkTasks => Set<WorkTask>();

    public WorkOrderDbContext(DbContextOptions<WorkOrderDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WorkOrder>(entity =>
        {
            entity.ToTable("WORK_ORDER");
            entity.HasKey(e => e.WorkOrderId);
            entity.Property(e => e.WorkOrderId).HasColumnName("WORK_ORDER_ID").ValueGeneratedOnAdd();
            entity.Property(e => e.WorkOrderName).HasColumnName("WORK_ORDER_NAME").HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            entity.Property(e => e.WorkOrderDescription).HasColumnName("WORK_ORDER_DESCRIPTION").HasColumnType("varchar(500)").HasMaxLength(500).IsRequired();
            entity.Property(e => e.DueDate).HasColumnName("DUE_DATE").HasColumnType("date").IsRequired();
            entity.Property(e => e.AssignedTo).HasColumnName("ASSIGNED_TO").IsRequired();
            entity.Property(e => e.WorkOrderStatus).HasColumnName("WORK_ORDER_STATUS")
                .HasColumnType("char(1)")
                .HasMaxLength(1)
                .IsRequired()
                .HasConversion(
                    v => v.Code.ToString(),
                    v => Domain.ValueObjects.WorkOrderStatus.FromCode(v[0]));
            entity.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
            entity.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
            entity.Property(e => e.UpdatedOn).HasColumnName("UPDATED_ON").HasColumnType("datetime2(3)");

            entity.HasMany(e => e.Tasks)
                .WithOne(e => e.WorkOrder)
                .HasForeignKey(e => e.WorkOrderId)
                .HasConstraintName("FK_WORK_TASK_WORK_ORDER")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.WorkOrderStatus).HasDatabaseName("IDX_WORK_ORDER_STATUS");
            entity.Ignore(e => e.DomainEvents);
        });

        modelBuilder.Entity<WorkTask>(entity =>
        {
            entity.ToTable("WORK_TASK");
            entity.HasKey(e => e.TaskId);
            entity.Property(e => e.TaskId).HasColumnName("TASK_ID").ValueGeneratedOnAdd();
            entity.Property(e => e.WorkOrderId).HasColumnName("WORK_ORDER_ID").IsRequired();
            entity.Property(e => e.TaskName).HasColumnName("TASK_NAME").HasColumnType("varchar(100)").HasMaxLength(100).IsRequired();
            entity.Property(e => e.AssignedTo).HasColumnName("ASSIGNED_TO").IsRequired();
            entity.Property(e => e.EstimatedHours).HasColumnName("ESTIMATED_HOURS").IsRequired();
            entity.Property(e => e.ActualHours).HasColumnName("ACTUAL_HOURS");
            entity.Property(e => e.TaskStatus).HasColumnName("TASK_STATUS")
                .HasColumnType("char(1)")
                .HasMaxLength(1)
                .IsRequired()
                .HasConversion(
                    v => v.Code.ToString(),
                    v => Domain.ValueObjects.WorkTaskStatus.FromCode(v[0]));
            entity.Property(e => e.CompletionRemarks).HasColumnName("COMPLETION_REMARKS").HasColumnType("varchar(500)").HasMaxLength(500);
            entity.Property(e => e.CompletedBy).HasColumnName("COMPLETED_BY");
            entity.Property(e => e.CompletedOn).HasColumnName("COMPLETED_ON").HasColumnType("datetime2(3)");
            entity.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
            entity.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").HasColumnType("datetime2(3)").IsRequired();
            entity.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
            entity.Property(e => e.UpdatedOn).HasColumnName("UPDATED_ON").HasColumnType("datetime2(3)");

            entity.HasIndex(e => e.WorkOrderId).HasDatabaseName("IDX_WORK_TASK_WORK_ORDER_ID");
            entity.HasIndex(e => e.TaskStatus).HasDatabaseName("IDX_WORK_TASK_STATUS");
            entity.Ignore(e => e.DomainEvents);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
