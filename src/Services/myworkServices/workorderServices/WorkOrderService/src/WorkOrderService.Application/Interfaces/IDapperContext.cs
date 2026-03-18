namespace WorkOrderService.Application.Interfaces;

public interface IDapperContext
{
    System.Data.IDbConnection CreateConnection();
}
