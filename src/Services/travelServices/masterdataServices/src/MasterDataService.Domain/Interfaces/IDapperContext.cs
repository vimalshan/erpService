namespace MasterDataService.Domain.Interfaces;

public interface IDapperContext
{
    System.Data.IDbConnection CreateConnection();
}
