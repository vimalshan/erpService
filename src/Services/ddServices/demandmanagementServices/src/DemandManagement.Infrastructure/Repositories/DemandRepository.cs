using DemandManagement.Domain.Entities;
using DemandManagement.Domain.Repositories;
using DemandManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace DemandManagement.Infrastructure.Repositories;

public class DemandRepository : IDemandRepository
{
    private readonly DemandDbContext _context;
    private readonly string _connectionString;

    public DemandRepository(DemandDbContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string not found.");
    }

    // Dapper for read (single record by ID)
    public async Task<DemandMaster> GetByIdAsync(long demandId)
    {
        const string sql = @"
            SELECT 
                DEMAND_ID         AS DemandId,
                DEMAND_TYPE       AS DemandType,
                DEPARTMENT_ID     AS DepartmentId,
                DEMAND_DESCRIPTION AS DemandDescription,
                REQUIRED_DATE     AS RequiredDate,
                PRIORITY          AS Priority,
                DEMAND_STATUS     AS DemandStatus,
                CREATED_BY        AS CreatedBy,
                CREATED_ON        AS CreatedOn,
                APPROVAL_REMARKS  AS ApprovalRemarks,
                APPROVED_BY       AS ApprovedBy,
                APPROVAL_DATE     AS ApprovalDate,
                COMPLETION_REMARKS AS CompletionRemarks,
                COMPLETED_BY      AS CompletedBy,
                COMPLETION_DATE   AS CompletionDate
            FROM DEMAND_MASTER
            WHERE DEMAND_ID = @Id";

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<DemandMaster>(sql, new { Id = demandId });
    }

    // EF Core for list query
    public async Task<IEnumerable<DemandMaster>> GetAllAsync() =>
        await _context.DemandMaster.ToListAsync();

    public async Task<long> AddAsync(DemandMaster demand)
    {
        _context.DemandMaster.Add(demand);
        await _context.SaveChangesAsync();
        return demand.DemandId;
    }

    public async Task UpdateAsync(DemandMaster demand)
    {
        _context.DemandMaster.Update(demand);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long demandId)
    {
        var demand = await _context.DemandMaster.FindAsync(demandId);
        if (demand is not null)
        {
            _context.DemandMaster.Remove(demand);
            await _context.SaveChangesAsync();
        }
    }
}
