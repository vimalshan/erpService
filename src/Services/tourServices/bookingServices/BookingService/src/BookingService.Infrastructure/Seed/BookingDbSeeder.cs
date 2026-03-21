using BookingService.Domain.Entities;
using BookingService.Infrastructure.Persistence;

namespace BookingService.Infrastructure.Seed;

public static class BookingDbSeeder
{
    public static async Task SeedAsync(BookingDbContext context)
    {
        if (context.BookRequestMains.Any())
            return;

        var bookings = new List<BookRequestMain>
        {
            new()
            {
                BookMainId = "BOOK-001",
                TpStatus = "Yes",
                TpId = "TP-001",
                EmployeeSysId = "EMP-001",
                Through = "Self",
                AdminId = "ADM-001",
                Remarks = "Business travel to Mumbai",
                Type = "TKT",
                ApprovalStatus = "APPROVED",
                ConfirmationStatus = "Confirmed",
                ProofType = "Aadhar",
                FoodPreference = "VEG",
                BudgetedCost = "15000",
                EnteredBy = "EMP-001",
                EnteredOn = DateTime.UtcNow.AddDays(-10),
                LastModifiedOn = DateTime.UtcNow.AddDays(-5),
                Tickets =
                [
                    new BookRequestTicket
                    {
                        BookTicketId = "TKT-001",
                        MainId = "BOOK-001",
                        ModeId = "AIR",
                        ClassId = "ECONOMY",
                        Type = "DEP",
                        StartDate = DateTime.UtcNow.AddDays(5),
                        StartTime = "FN",
                        StartCityId = "DEL",
                        StartCity = "New Delhi",
                        EndCityId = "BOM",
                        EndCity = "Mumbai",
                        ConfirmationNo = "CNF-AIR-001",
                        ApprovalStatus = "APPROVED",
                        LastModifiedBy = "EMP-001",
                        LastModifiedOn = DateTime.UtcNow.AddDays(-5),
                        BudgetCost = "8000",
                        AdminRemarks = "",
                        SpecialSanction = "N",
                        SpecialSanctionReason = ""
                    }
                ]
            },
            new()
            {
                BookMainId = "BOOK-002",
                TpStatus = "No",
                TpId = "TP-002",
                EmployeeSysId = "EMP-002",
                Through = "Admin",
                AdminId = "ADM-001",
                Remarks = "Client visit hotel stay",
                Type = "STY",
                ApprovalStatus = "PENDING",
                ConfirmationStatus = "Pending",
                FoodPreference = "NON-VEG",
                BudgetedCost = "5000",
                EnteredBy = "ADM-001",
                EnteredOn = DateTime.UtcNow.AddDays(-3),
                LastModifiedOn = DateTime.UtcNow.AddDays(-1),
                Stays =
                [
                    new BookRequestStay
                    {
                        BookStayId = "STY-001",
                        MainId = "BOOK-002",
                        CityId = "BLR",
                        City = "Bengaluru",
                        CheckInDate = DateTime.UtcNow.AddDays(7),
                        CheckOutDate = DateTime.UtcNow.AddDays(9),
                        ConfirmationNo = "",
                        LastModifiedBy = "ADM-001",
                        LastModifiedOn = DateTime.UtcNow.AddDays(-1)
                    }
                ]
            },
            new()
            {
                BookMainId = "BOOK-003",
                TpStatus = "Yes",
                TpId = "TP-003",
                EmployeeSysId = "EMP-003",
                Through = "Self",
                AdminId = "ADM-002",
                Remarks = "Airport cab pickup",
                Type = "CAB",
                ApprovalStatus = "APPROVED",
                ConfirmationStatus = "Confirmed",
                BudgetedCost = "2000",
                EnteredBy = "EMP-003",
                EnteredOn = DateTime.UtcNow.AddDays(-7),
                LastModifiedOn = DateTime.UtcNow.AddDays(-2),
                Cabs =
                [
                    new BookRequestCab
                    {
                        BookCabId = "CAB-001",
                        MainId = "BOOK-003",
                        PickupLocation = "Bengaluru Airport",
                        DropLocation = "Electronic City",
                        PickupDate = DateTime.UtcNow.AddDays(3),
                        CarType = "Sedan",
                        Preference = "AC",
                        TripType = "OFF",
                        Address = "Terminal 1, Kempegowda International Airport",
                        ConfirmationNo = "CAB-CNF-001",
                        LastModifiedBy = "EMP-003",
                        LastModifiedOn = DateTime.UtcNow.AddDays(-2),
                        Nature = "Airport Transfer"
                    }
                ]
            }
        };

        await context.BookRequestMains.AddRangeAsync(bookings);
        await context.SaveChangesAsync();
    }
}
