using Microsoft.EntityFrameworkCore;
using TourServices.Infrastructure.Persistence;

namespace TourServices.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.TourPackages.AnyAsync()) return; // Already seeded

        var sql = """
            IF NOT EXISTS (SELECT 1 FROM TOUR_PACKAGE WHERE TOUR_NAME = 'Golden Triangle Tour')
            BEGIN
                INSERT INTO TOUR_PACKAGE (TOUR_NAME, DESTINATION, START_DATE, END_DATE,
                    TOUR_PACKAGE_COST, MAX_PARTICIPANTS, TOUR_STATUS, CREATED_BY, CREATED_ON)
                VALUES
                    ('Golden Triangle Tour', 'Delhi-Agra-Jaipur', '2026-06-01', '2026-06-07', 25000, 20, 'A', 1, GETDATE()),
                    ('Himalayan Adventure', 'Manali-Leh-Ladakh', '2026-07-15', '2026-07-25', 45000, 15, 'P', 1, GETDATE()),
                    ('Coastal Karnataka Tour', 'Mangalore-Goa', '2026-05-10', '2026-05-15', 18000, 25, 'A', 1, GETDATE());
            END
            """;

        await context.Database.ExecuteSqlRawAsync(sql);
    }
}
