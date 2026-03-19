using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace VehicleTracking.Infrastructure.Dapper;

public interface IDapperQueryService
{
    Task<long> RegisterVehicleAsync(string regNum1, string? regNum2, string? regNum3, string regNum4,
        DateTime regDate, string updatedBy, long updatedByNum, CancellationToken ct = default);
    Task UpdateVehicleStageAsync(long trackingNumber, long vehicleTracker, long stageCode,
        char stageDecision, string enteredBy, long enteredByNum, CancellationToken ct = default);
    Task<IEnumerable<dynamic>> GetVehicleStagesAsync(long trackingNumber, CancellationToken ct = default);
}

public class DapperQueryService(IConfiguration configuration) : IDapperQueryService
{
    private IDbConnection CreateConnection()
        => new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

    public async Task<long> RegisterVehicleAsync(string regNum1, string? regNum2, string? regNum3, string regNum4,
        DateTime regDate, string updatedBy, long updatedByNum, CancellationToken ct = default)
    {
        using var connection = CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@p_RegNum1", regNum1);
        parameters.Add("@p_RegNum2", regNum2);
        parameters.Add("@p_RegNum3", regNum3);
        parameters.Add("@p_RegNum4", regNum4);
        parameters.Add("@p_RegDate", regDate);
        parameters.Add("@p_UpdatedBy", updatedBy);
        parameters.Add("@p_UpdatedByNum", updatedByNum);
        parameters.Add("@p_VehicleID", dbType: DbType.Int64, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            new CommandDefinition("dbo.usp_RegisterVehicle", parameters, commandType: CommandType.StoredProcedure, cancellationToken: ct));

        return parameters.Get<long>("@p_VehicleID");
    }

    public async Task UpdateVehicleStageAsync(long trackingNumber, long vehicleTracker, long stageCode,
        char stageDecision, string enteredBy, long enteredByNum, CancellationToken ct = default)
    {
        using var connection = CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@p_TrackingNumber", trackingNumber);
        parameters.Add("@p_VehicleTracker", vehicleTracker);
        parameters.Add("@p_StageCode", stageCode);
        parameters.Add("@p_StageDecision", stageDecision.ToString());
        parameters.Add("@p_EnteredBy", enteredBy);
        parameters.Add("@p_EnteredByNum", enteredByNum);

        await connection.ExecuteAsync(
            new CommandDefinition("dbo.usp_UpdateVehicleStage", parameters, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<IEnumerable<dynamic>> GetVehicleStagesAsync(long trackingNumber, CancellationToken ct = default)
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync(
            new CommandDefinition("dbo.usp_GetVehicleStages",
                new { p_TrackingNumber = trackingNumber },
                commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }
}
