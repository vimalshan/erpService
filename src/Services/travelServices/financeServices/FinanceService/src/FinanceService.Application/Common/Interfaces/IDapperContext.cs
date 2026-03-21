using System.Data;

namespace FinanceService.Application.Common.Interfaces;

public interface IDapperContext
{
    IDbConnection CreateConnection();
}
