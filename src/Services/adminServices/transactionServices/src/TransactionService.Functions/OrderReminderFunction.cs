using System.Data;
using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace TransactionService.Functions;

public sealed class OrderReminderFunction
{
    private readonly ILogger<OrderReminderFunction> _logger;

    public OrderReminderFunction(ILogger<OrderReminderFunction> logger)
    {
        _logger = logger;
    }

    [Function("OrderDeliveryReminder")]
    public async Task Run(
        [TimerTrigger("0 0 8 * * 1-5")] TimerInfo timerInfo) // Weekdays at 8 AM
    {
        _logger.LogInformation("Order delivery reminder check started at {Time}", DateTime.UtcNow);

        var connectionString = Environment.GetEnvironmentVariable("TransactionDb");
        if (string.IsNullOrEmpty(connectionString))
        {
            _logger.LogError("TransactionDb connection string not configured");
            return;
        }

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        // Find overdue orders (delivery date passed, not yet received)
        var overdueOrders = await connection.QueryAsync<dynamic>(
            @"SELECT om.OM_ORDERMAIN_ID, om.OM_VENDORID, om.OM_DELIVERYDATE,
                     os.OS_ORDERSUB_ID, os.OS_ORDERED_QTY
              FROM SP_ORDER_MAIN om
              INNER JOIN SP_ORDER_SUB os ON om.OM_ORDERMAIN_ID = os.OS_ORDERMAIN_ID
              WHERE om.OM_DELIVERYDATE < GETDATE()
                AND os.OS_RECEIVEDON IS NULL");

        foreach (var order in overdueOrders)
        {
            _logger.LogWarning(
                "OVERDUE ORDER: Order {OrderId} Sub {SubId} from Vendor {VendorId} - Due: {DueDate}, Qty: {Qty}",
                (object)order.OM_ORDERMAIN_ID, (object)order.OS_ORDERSUB_ID, (object)order.OM_VENDORID,
                (object)order.OM_DELIVERYDATE, (object)order.OS_ORDERED_QTY);
        }

        // Find pending requests older than 7 days
        var staleRequests = await connection.QueryAsync<dynamic>(
            @"SELECT rm.RM_REQUESTID, rm.RM_REQUESTEDBY, rm.RM_REQUESTEDON,
                     COUNT(rs.RS_REQUESTSUB_ID) AS PendingItems
              FROM SP_REQUEST_MAIN rm
              INNER JOIN SP_REQUEST_SUB rs ON rm.RM_REQUESTID = rs.RS_REQUESTID
              WHERE rs.RS_STATUS = 'P'
                AND rm.RM_REQUESTEDON < DATEADD(DAY, -7, GETDATE())
              GROUP BY rm.RM_REQUESTID, rm.RM_REQUESTEDBY, rm.RM_REQUESTEDON");

        foreach (var req in staleRequests)
        {
            _logger.LogWarning(
                "STALE REQUEST: Request {RequestId} by Employee {EmpId} on {Date} has {Count} pending items",
                (object)req.RM_REQUESTID, (object)req.RM_REQUESTEDBY, (object)req.RM_REQUESTEDON, (object)req.PendingItems);
        }

        _logger.LogInformation(
            "Reminder check completed. {OverdueCount} overdue orders, {StaleCount} stale requests.",
            overdueOrders.Count(), staleRequests.Count());
    }
}
