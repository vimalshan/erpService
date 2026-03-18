using MedicineManagement.Domain.Entities;
using MedicineManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MedicineManagement.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<MedicineManagementDbContext>();
        await context.Database.MigrateAsync();

        if (!await context.MedicineTypes.AnyAsync())
        {
            var types = new[]
            {
                MedicineType.Create("TAB", "Tablet", "SYSTEM", 0),
                MedicineType.Create("CAP", "Capsule", "SYSTEM", 0),
                MedicineType.Create("SYR", "Syrup", "SYSTEM", 0),
                MedicineType.Create("INJ", "Injection", "SYSTEM", 0),
                MedicineType.Create("CRM", "Cream", "SYSTEM", 0),
                MedicineType.Create("DRP", "Drops", "SYSTEM", 0),
            };
            foreach (var t in types) t.ClearDomainEvents();
            await context.MedicineTypes.AddRangeAsync(types);
        }

        if (!await context.MedicinePackagings.AnyAsync())
        {
            var pkgs = new[]
            {
                MedicinePackaging.Create("BOX", "Box", "SYSTEM", 0),
                MedicinePackaging.Create("STR", "Strip", "SYSTEM", 0),
                MedicinePackaging.Create("BTL", "Bottle", "SYSTEM", 0),
                MedicinePackaging.Create("AMP", "Ampoule", "SYSTEM", 0),
                MedicinePackaging.Create("TUB", "Tube", "SYSTEM", 0),
            };
            await context.MedicinePackagings.AddRangeAsync(pkgs);
        }

        if (!await context.Medicines.AnyAsync())
        {
            var medicines = new[]
            {
                Medicine.Create("PCM", "Paracetamol 500mg", "TAB", 'H', 100, 1000, "SYSTEM", 0),
                Medicine.Create("AMX", "Amoxicillin 250mg", "CAP", 'H', 50, 500, "SYSTEM", 0),
                Medicine.Create("IBP", "Ibuprofen 400mg", "TAB", 'M', 50, 500, "SYSTEM", 0),
                Medicine.Create("CPH", "Cough Syrup", "SYR", 'M', 20, 200, "SYSTEM", 0),
                Medicine.Create("INS", "Insulin Injection", "INJ", 'H', 10, 100, "SYSTEM", 0),
            };
            foreach (var m in medicines) m.ClearDomainEvents();
            await context.Medicines.AddRangeAsync(medicines);
        }

        if (!await context.DoctorAttendants.AnyAsync())
        {
            var docs = new[]
            {
                DoctorAttendant.Create("DOC001", 'D', "Dr. Ahmed"),
                DoctorAttendant.Create("DOC002", 'D', "Dr. Fatima"),
                DoctorAttendant.Create("ATT001", 'A', "Nurse Sara"),
            };
            await context.DoctorAttendants.AddRangeAsync(docs);
        }

        await context.SaveChangesAsync();
    }
}
