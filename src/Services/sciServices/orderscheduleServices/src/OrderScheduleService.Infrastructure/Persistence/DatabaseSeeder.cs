namespace OrderScheduleService.Infrastructure.Persistence;

using OrderScheduleService.Domain.Aggregates;
using OrderScheduleService.Domain.Entities;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(OrderScheduleDbContext context)
    {
        #region Seed Tied Orders
        if (!context.TiedOrders.Any())
        {
            var order1 = new TiedOrderAggregate(
                orderId: 0,
                customerCode: "CUST001",
                companyUnitId: 1,
                orderedDate: DateTime.UtcNow.AddDays(-5),
                modifiedUserId: "ADMIN");

            order1.AddDetail(
                itemId: 1001,
                itemName: "Bottled Water 500ML",
                orderQuantity: 1000,
                dispatchDate: DateTime.UtcNow.AddDays(2),
                price: 25.00m);

            order1.AddDetail(
                itemId: 1002,
                itemName: "Bottled Water 1L",
                orderQuantity: 500,
                dispatchDate: DateTime.UtcNow.AddDays(3),
                price: 35.00m);

            var order2 = new TiedOrderAggregate(
                orderId: 0,
                customerCode: "CUST002",
                companyUnitId: 1,
                orderedDate: DateTime.UtcNow.AddDays(-3),
                modifiedUserId: "ADMIN");

            order2.AddDetail(
                itemId: 1001,
                itemName: "Bottled Water 500ML",
                orderQuantity: 750,
                dispatchDate: DateTime.UtcNow.AddDays(1),
                price: 25.00m);

            var order3 = new TiedOrderAggregate(
                orderId: 0,
                customerCode: "CUST003",
                companyUnitId: 1,
                orderedDate: DateTime.UtcNow.AddDays(-1),
                modifiedUserId: "ADMIN");

            order3.AddDetail(
                itemId: 1003,
                itemName: "Mineral Water 1.5L",
                orderQuantity: 2000,
                dispatchDate: DateTime.UtcNow.AddDays(5),
                price: 45.00m);

            order3.AddDetail(
                itemId: 1004,
                itemName: "Sparkling Water 500ML",
                orderQuantity: 300,
                dispatchDate: DateTime.UtcNow.AddDays(4),
                price: 30.00m);

            await context.TiedOrders.AddRangeAsync(new[] { order1, order2, order3 });
        }
        #endregion

        #region Seed Schedules
        if (!context.Schedules.Any())
        {
            var schedule1 = new ScheduleAggregate(
                scheduleId: 0,
                fillingPointGroupId: 1,
                itemId: 1001,
                orderType: "T",
                orderId: 1,
                orderLineId: 1,
                requiredDate: DateTime.UtcNow.AddDays(2),
                orderQuantity: 1000,
                shiftCapacity: 500);

            schedule1.AddScheduleDetail(
                fillingDate: DateTime.UtcNow.AddDays(2),
                fillingShift: 'A',
                startTime: "06:00",
                endTime: "14:00",
                fillQuantity: 500,
                fillingPointGroupId: 1);

            var schedule2 = new ScheduleAggregate(
                scheduleId: 0,
                fillingPointGroupId: 1,
                itemId: 1002,
                orderType: "T",
                orderId: 1,
                orderLineId: 2,
                requiredDate: DateTime.UtcNow.AddDays(3),
                orderQuantity: 500,
                shiftCapacity: 500);

            schedule2.AddScheduleDetail(
                fillingDate: DateTime.UtcNow.AddDays(3),
                fillingShift: 'B',
                startTime: "14:00",
                endTime: "22:00",
                fillQuantity: 500,
                fillingPointGroupId: 1);

            var schedule3 = new ScheduleAggregate(
                scheduleId: 0,
                fillingPointGroupId: 2,
                itemId: 1003,
                orderType: "T",
                orderId: 3,
                orderLineId: 1,
                requiredDate: DateTime.UtcNow.AddDays(5),
                orderQuantity: 2000,
                shiftCapacity: 1000);

            schedule3.AddScheduleDetail(
                fillingDate: DateTime.UtcNow.AddDays(5),
                fillingShift: 'A',
                startTime: "06:00",
                endTime: "14:00",
                fillQuantity: 1000,
                fillingPointGroupId: 2);

            schedule3.AddScheduleDetail(
                fillingDate: DateTime.UtcNow.AddDays(5),
                fillingShift: 'B',
                startTime: "14:00",
                endTime: "22:00",
                fillQuantity: 1000,
                fillingPointGroupId: 2);

            await context.Schedules.AddRangeAsync(new[] { schedule1, schedule2, schedule3 });
        }
        #endregion

        #region Seed Order Actuals
        if (!context.OrderActuals.Any())
        {
            var actuals = new List<OrderActual>
            {
                new OrderActual(
                    orderNumber: 5001,
                    lineId: 1,
                    orderedQuantity: 1000,
                    requestDate: DateTime.UtcNow.AddDays(2),
                    scheduleShipDate: DateTime.UtcNow.AddDays(3),
                    orderedItem: "Bottled Water 500ML")
                {
                    HeaderId = 100,
                    OrderQuantityUom = "EA",
                    CustomerName = "ABC Distributors",
                    ShipFromOrgId = 1,
                    SoldToOrgId = 201
                },
                new OrderActual(
                    orderNumber: 5001,
                    lineId: 2,
                    orderedQuantity: 500,
                    requestDate: DateTime.UtcNow.AddDays(3),
                    scheduleShipDate: DateTime.UtcNow.AddDays(4),
                    orderedItem: "Bottled Water 1L")
                {
                    HeaderId = 100,
                    OrderQuantityUom = "EA",
                    CustomerName = "ABC Distributors",
                    ShipFromOrgId = 1,
                    SoldToOrgId = 201
                },
                new OrderActual(
                    orderNumber: 5002,
                    lineId: 1,
                    orderedQuantity: 750,
                    requestDate: DateTime.UtcNow.AddDays(1),
                    scheduleShipDate: DateTime.UtcNow.AddDays(2),
                    orderedItem: "Bottled Water 500ML")
                {
                    HeaderId = 101,
                    OrderQuantityUom = "EA",
                    CustomerName = "XYZ Trading",
                    ShipFromOrgId = 1,
                    SoldToOrgId = 202
                }
            };

            await context.OrderActuals.AddRangeAsync(actuals);
        }
        #endregion

        #region Seed Empties Orders
        if (!context.EmptiesOrders.Any())
        {
            var emptiesOrders = new List<EmptiesOrder>
            {
                new EmptiesOrder(
                    sciItemId: 1001,
                    itemId: 2001,
                    orderQuantity: 500,
                    needDate: DateTime.UtcNow.AddDays(1),
                    orderDate: DateTime.UtcNow,
                    entryDate: DateTime.UtcNow),
                new EmptiesOrder(
                    sciItemId: 1002,
                    itemId: 2002,
                    orderQuantity: 300,
                    needDate: DateTime.UtcNow.AddDays(2),
                    orderDate: DateTime.UtcNow,
                    entryDate: DateTime.UtcNow)
            };

            await context.EmptiesOrders.AddRangeAsync(emptiesOrders);
        }
        #endregion

        #region Seed Schedule Confirms
        if (!context.ScheduleConfirms.Any())
        {
            var confirms = new List<ScheduleConfirm>
            {
                new ScheduleConfirm(
                    scheduleDate: DateTime.UtcNow.Date,
                    scheduleStatus: "C",
                    modifiedDate: DateTime.UtcNow),
                new ScheduleConfirm(
                    scheduleDate: DateTime.UtcNow.Date.AddDays(1),
                    scheduleStatus: "P",
                    modifiedDate: DateTime.UtcNow)
            };

            await context.ScheduleConfirms.AddRangeAsync(confirms);
        }
        #endregion

        await context.SaveChangesAsync();
    }
}
