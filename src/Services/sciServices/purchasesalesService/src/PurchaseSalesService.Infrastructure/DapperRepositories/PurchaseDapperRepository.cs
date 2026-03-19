using Microsoft.Extensions.Configuration;

namespace PurchaseSalesService.Infrastructure.DapperRepositories;

public sealed class PurchaseDapperRepository : DapperBase
{
    public PurchaseDapperRepository(IConfiguration configuration) : base(configuration) { }

    public Task<IEnumerable<dynamic>> GetActivePurchasesAsync()
        => QueryAsync<dynamic>(
            "SELECT * FROM PURCHASE_DETAILS WHERE PD_CAN_FLG <> 'Y' OR PD_CAN_FLG IS NULL");

    public Task<dynamic?> GetPurchaseWithLogsAsync(long serialNumber)
        => QueryFirstOrDefaultAsync<dynamic>(
            "SELECT pd.*, lp.PD_MOD_USR, lp.PD_MOD_DAT " +
            "FROM PURCHASE_DETAILS pd " +
            "LEFT JOIN LOG_PURCHASE_DETAILS lp ON pd.PD_SRL_NUM = lp.PD_SRL_NUM " +
            "WHERE pd.PD_SRL_NUM = @srl",
            new { srl = serialNumber });

    /// <summary>Delegates to the usp_RecordPurchase stored procedure.</summary>
    public Task<int> RecordPurchaseViaProcAsync(
        long trackingNum, string? supplierCode, long purposeCode,
        long stageCode, string enteredBy, long enteredByNum)
        => ExecuteAsync("EXEC dbo.usp_RecordPurchase @p_TrackingNum, @p_SupplierCode, " +
            "@p_PurposeCode, @p_StageCode, @p_EnteredBy, @p_EnteredByNum",
            new { p_TrackingNum = trackingNum, p_SupplierCode = supplierCode,
                  p_PurposeCode = purposeCode, p_StageCode = stageCode,
                  p_EnteredBy = enteredBy, p_EnteredByNum = enteredByNum });
}
