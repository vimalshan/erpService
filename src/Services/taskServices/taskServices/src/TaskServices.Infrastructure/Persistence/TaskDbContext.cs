using Microsoft.EntityFrameworkCore;
using TaskServices.Domain.Entities;

namespace TaskServices.Infrastructure.Persistence;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

    public DbSet<TaskMail> TaskMails => Set<TaskMail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskMail>(entity =>
        {
            entity.ToTable("TASK_MAIL");
            entity.HasKey(e => e.MID);
            entity.Property(e => e.MID).HasColumnName("MID").HasColumnType("decimal(38,0)");
            entity.Property(e => e.SYSID).HasColumnName("SYSID").HasColumnType("decimal(38,0)");
        });

        // Seed data
        modelBuilder.Entity<TaskMail>().HasData(
            new { MID = 1m, SYSID = 1001m },
            new { MID = 2m, SYSID = 1002m },
            new { MID = 3m, SYSID = 1001m }
        );

        base.OnModelCreating(modelBuilder);
    }
}
