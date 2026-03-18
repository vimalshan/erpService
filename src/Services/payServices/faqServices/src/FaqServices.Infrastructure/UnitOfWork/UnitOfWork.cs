using FaqServices.Domain.Interfaces;
using FaqServices.Infrastructure.Data;
using FaqServices.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace FaqServices.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly FaqDbContext _context;
    private IDbContextTransaction? _transaction;
    private IFaqGradeRepository? _faqGradeRepository;
    private IFaqQuestionRepository? _faqQuestionRepository;
    private IFaqAnswerRepository? _faqAnswerRepository;

    public UnitOfWork(FaqDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IFaqGradeRepository FaqGrades =>
        _faqGradeRepository ??= new FaqGradeRepository(_context);

    public IFaqQuestionRepository FaqQuestions =>
        _faqQuestionRepository ??= new FaqQuestionRepository(_context);

    public IFaqAnswerRepository FaqAnswers =>
        _faqAnswerRepository ??= new FaqAnswerRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        try
        {
            await SaveChangesAsync(ct);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(ct);
            }
        }
        catch
        {
            await RollbackTransactionAsync(ct);
            throw;
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

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(ct);
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

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
        await _context.DisposeAsync();
    }
}
