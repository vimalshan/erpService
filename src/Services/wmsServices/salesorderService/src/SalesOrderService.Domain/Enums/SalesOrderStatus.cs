namespace SalesOrderService.Domain.Enums;

public enum SalesOrderStatus
{
    Draft = 0,
    Confirmed = 1,
    Picking = 2,
    Shipping = 3,
    Completed = 4,
    Cancelled = 5
}
