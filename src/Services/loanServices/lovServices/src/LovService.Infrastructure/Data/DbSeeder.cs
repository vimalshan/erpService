using LovService.Domain.Entities;
using LovService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LovService.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(LovDbContext db)
    {
        await db.Database.MigrateAsync();

        if (!await db.LovTypeMasts.AnyAsync())
        {
            var types = new[]
            {
                LovTypeMast.Create(1, "Loan Status",       'F', 1),
                LovTypeMast.Create(2, "Repayment Frequency",'V', 1),
                LovTypeMast.Create(3, "Interest Type",     'F', 1),
                LovTypeMast.Create(4, "Collateral Type",   'V', 1),
            };

            db.LovTypeMasts.AddRange(types);
            await db.SaveChangesAsync();
        }

        if (!await db.LovMasters.AnyAsync())
        {
            var seed = new (int typeId, string name)[]
            {
                (1, "Active"), (1, "Closed"), (1, "Defaulted"), (1, "Pending Approval"),
                (2, "Monthly"), (2, "Quarterly"), (2, "Annually"),
                (3, "Fixed"), (3, "Variable"),
                (4, "Property"), (4, "Vehicle"), (4, "Gold"),
            };

            long id = 1;
            foreach (var (typeId, name) in seed)
                db.LovMasters.Add(LovMaster.Create(id++, typeId, name, 1));

            await db.SaveChangesAsync();
        }

        if (!await db.ProgramLovMasts.AnyAsync())
        {
            var programs = new[]
            {
                ProgramLovMast.Create("LOAN_TYPE", "PL", "Personal Loan"),
                ProgramLovMast.Create("LOAN_TYPE", "HL", "Home Loan"),
                ProgramLovMast.Create("LOAN_TYPE", "AL", "Auto Loan"),
                ProgramLovMast.Create("LOAN_TYPE", "BL", "Business Loan"),
                ProgramLovMast.Create("GENDER",    "M",  "Male"),
                ProgramLovMast.Create("GENDER",    "F",  "Female"),
                ProgramLovMast.Create("GENDER",    "O",  "Other"),
            };

            db.ProgramLovMasts.AddRange(programs);
            await db.SaveChangesAsync();
        }
    }
}
