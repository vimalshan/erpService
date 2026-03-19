using Microsoft.Extensions.Configuration;

namespace PurchaseSalesService.Infrastructure.DapperRepositories;

public sealed class SaleDapperRepository : DapperBase
{
    public SaleDapperRepository(IConfiguration configuration) : base(configuration) { }

    public Task<IEnumerable<dynamic>> GetActiveSalesAsync()
        => QueryAsync<dynamic>(
            "SELECT sm.*, ss.SS_PRO_COD, ss.SS_PRO_QTN " +
            "FROM SALE_MAIN sm LEFT JOIN SALE_SUB ss ON sm.SL_SER_NUM = ss.SS_SER_NUM " +
            "WHERE sm.SL_CAN_FLG <> 'Y' OR sm.SL_CAN_FLG IS NULL");

    /// <summary>Delegates to the usp_RecordSale stored procedure.</summary>
    public Task<int> RecordSaleViaProcAsync(
        long trackingNum, long purposeCode, long stageCode,
        string productCode, string enteredBy, long enteredByNum)
        => ExecuteAsync("EXEC dbo.usp_RecordSale @p_TrackingNum, @p_PurposeCode, " +
            "@p_StageCode, @p_ProductCode, @p_EnteredBy, @p_EnteredByNum",
            new { p_TrackingNum = trackingNum, p_PurposeCode = purposeCode,
                  p_StageCode = stageCode, p_ProductCode = productCode,
                  p_EnteredBy = enteredBy, p_EnteredByNum = enteredByNum });
}
