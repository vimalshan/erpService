namespace ApprovalService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Interfaces;
using ApprovalService.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

/// <summary>
/// Repository for Approval Master aggregate
/// </summary>
public class ApprovalMasterRepository : IApprovalMasterRepository
{
    private readonly ApprovalServiceDbContext _context;
    private readonly ILogger<ApprovalMasterRepository> _logger;

    public ApprovalMasterRepository(ApprovalServiceDbContext context, ILogger<ApprovalMasterRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApprovalMaster?> GetByIdAsync(long id)
    {
        try
        {
            return await _context.ApprovalMasters
                .Include(am => am.Approvers)
                .FirstOrDefaultAsync(am => am.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approval master by ID {Id}", id);
            throw;
        }
    }

    public async Task<ApprovalMaster?> GetByCodeAsync(string code)
    {
        try
        {
            return await _context.ApprovalMasters
                .Include(am => am.Approvers)
                .FirstOrDefaultAsync(am => am.Code == code);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approval master by code {Code}", code);
            throw;
        }
    }

    public async Task<IEnumerable<ApprovalMaster>> GetByModuleAsync(string module)
    {
        try
        {
            return await _context.ApprovalMasters
                .Where(am => am.Module == module)
                .Include(am => am.Approvers)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approval masters by module {Module}", module);
            throw;
        }
    }

    public async Task<IEnumerable<ApprovalMaster>> GetAllAsync()
    {
        try
        {
            return await _context.ApprovalMasters
                .Include(am => am.Approvers)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all approval masters");
            throw;
        }
    }

    public async Task AddAsync(ApprovalMaster approval)
    {
        try
        {
            await _context.ApprovalMasters.AddAsync(approval);
            _logger.LogInformation("Approval master queued for addition");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding approval master");
            throw;
        }
    }

    public async Task UpdateAsync(ApprovalMaster approval)
    {
        try
        {
            _context.ApprovalMasters.Update(approval);
            _logger.LogInformation("Approval master queued for update: {Id}", approval.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating approval master");
            throw;
        }
    }

    public async Task DeleteAsync(long id)
    {
        try
        {
            var approval = await GetByIdAsync(id);
            if (approval == null)
            {
                throw new KeyNotFoundException($"Approval master with ID {id} not found");
            }

            _context.ApprovalMasters.Remove(approval);
            _logger.LogInformation("Approval master queued for deletion: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting approval master");
            throw;
        }
    }
}

/// <summary>
/// Repository for Approver Employee aggregate
/// </summary>
public class ApproverEmployeeRepository : IApproverEmployeeRepository
{
    private readonly ApprovalServiceDbContext _context;
    private readonly ILogger<ApproverEmployeeRepository> _logger;

    public ApproverEmployeeRepository(ApprovalServiceDbContext context, ILogger<ApproverEmployeeRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApproverEmployee?> GetByIdAsync(long id)
    {
        try
        {
            return await _context.ApproverEmployees
                .FirstOrDefaultAsync(ae => ae.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approver employee by ID {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ApproverEmployee>> GetByApprovalMasterAsync(long approvalMasterId)
    {
        try
        {
            return await _context.ApproverEmployees
                .Where(ae => ae.ApprovalMasterId == approvalMasterId)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approver employees by approval master {ApprovalMasterId}", approvalMasterId);
            throw;
        }
    }

    public async Task<IEnumerable<ApproverEmployee>> GetByEmployeeAsync(long employeeSysId)
    {
        try
        {
            return await _context.ApproverEmployees
                .Where(ae => ae.EmployeeSysId == employeeSysId)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving approver employees by employee {EmployeeSysId}", employeeSysId);
            throw;
        }
    }

    public async Task AddAsync(ApproverEmployee approver)
    {
        try
        {
            await _context.ApproverEmployees.AddAsync(approver);
            _logger.LogInformation("Approver employee queued for addition");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding approver employee");
            throw;
        }
    }

    public async Task UpdateAsync(ApproverEmployee approver)
    {
        try
        {
            _context.ApproverEmployees.Update(approver);
            _logger.LogInformation("Approver employee queued for update: {Id}", approver.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating approver employee");
            throw;
        }
    }

    public async Task DeleteAsync(long id)
    {
        try
        {
            var approver = await GetByIdAsync(id);
            if (approver == null)
            {
                throw new KeyNotFoundException($"Approver employee with ID {id} not found");
            }

            _context.ApproverEmployees.Remove(approver);
            _logger.LogInformation("Approver employee queued for deletion: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting approver employee");
            throw;
        }
    }
}
