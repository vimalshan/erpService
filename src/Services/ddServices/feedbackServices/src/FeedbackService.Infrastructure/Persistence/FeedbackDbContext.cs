namespace FeedbackService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Domain.Aggregates;
using Domain.Entities;
using Application.Commands.Handlers;

/// <summary>
/// Database context for the Feedback microservice
/// </summary>
public class FeedbackDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    /// <summary>
    /// Initializes a new instance of the FeedbackDbContext class
    /// </summary>
    public FeedbackDbContext(DbContextOptions<FeedbackDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets the Feedback DbSet
    /// </summary>
    public DbSet<Feedback> Feedbacks { get; set; } = null!;

    /// <summary>
    /// Gets the FeedbackItem DbSet
    /// </summary>
    public DbSet<FeedbackItem> FeedbackItems { get; set; } = null!;

    /// <summary>
    /// Begins a transaction
    /// </summary>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    /// <summary>
    /// Configures the database model
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ignore domain event classes - they are not persisted directly
        modelBuilder.Ignore<Domain.Common.DomainEvent>();

        modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
        modelBuilder.ApplyConfiguration(new FeedbackItemConfiguration());

        // Configure LOV_FEEDBACK table (read-only)
        modelBuilder.Entity("LOVFeedback", b =>
        {
            b.ToTable("LOV_FEEDBACK");
            b.HasNoKey();
            b.Property<decimal?>("DDFeedbackId").HasColumnName("DD_FEEDBACKID");
            b.Property<string?>("DDFeedbackName").HasColumnName("DD_FEEDBACKNAME").HasMaxLength(400);
        });
    }
}
