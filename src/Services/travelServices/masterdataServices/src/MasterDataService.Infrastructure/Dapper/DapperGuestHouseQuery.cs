using Dapper;
using MasterDataService.Application.DTOs;
using MasterDataService.Domain.Interfaces;

namespace MasterDataService.Infrastructure.Dapper;

public class DapperGuestHouseQuery
{
    private readonly IDapperContext _context;

    public DapperGuestHouseQuery(IDapperContext context) => _context = context;

    public async Task<IEnumerable<GuestHouseDto>> GetGuestHouseListAsync(long? adminCode = null)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<GuestHouseDto>(
            "usp_GetGuestHouseList",
            new { p_AdminCode = adminCode },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
