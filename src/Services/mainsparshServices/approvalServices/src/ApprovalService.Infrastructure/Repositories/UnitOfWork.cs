namespace ApprovalService.Infrastructure.Repositories;

using ApprovalService.Domain.Interfaces;
using ApprovalService.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

/// <summary>
/// Unit of Work pattern implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApprovalServiceDbContext _context;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<UnitOfWork> _logger;
    private IApprovalMasterRepository? _approvalMasterRepository;
    private IApproverEmployeeRepository? _approverEmployeeRepository;

    public UnitOfWork(ApprovalServiceDbContext context, ILogger<UnitOfWork> logger, ILoggerFactory loggerFactory)
    {
        _context = context;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    public IApprovalMasterRepository ApprovalMasters =>
        _approvalMasterRepository ??= new ApprovalMasterRepository(_context, _loggerFactory.CreateLogger<ApprovalMasterRepository>());

    public IApproverEmployeeRepository ApproverEmployees =>
        _approverEmployeeRepository ??= new ApproverEmployeeRepository(_context, _loggerFactory.CreateLogger<ApproverEmployeeRepository>());

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Saving changes to database");
            var result = await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Saved {Count} changes to database", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving changes to database");
            throw;
        }
    }
}
