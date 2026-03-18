using AccidentManagementService.Domain.Entities;
using AccidentManagementService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AccidentManagementService.Infrastructure.Data
{
    /// <summary>
    /// Service for seeding the database with initial test data
    /// </summary>
    public class DataSeedingService
    {
        private readonly AccidentManagementDbContext _context;
        private readonly ILogger<DataSeedingService> _logger;

        public DataSeedingService(AccidentManagementDbContext context, ILogger<DataSeedingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Seed the database with initial data if it's empty
        /// </summary>
        public async Task SeedAsync()
        {
            try
            {
                _logger.LogInformation("Starting database seeding...");

                // Apply any pending migrations
                _logger.LogInformation("Applying pending migrations...");
                await _context.Database.MigrateAsync();
                _logger.LogInformation("Migrations applied successfully");

                // Seed reference data - only seed if tables are empty
                await SeedInjuryCategories();
                await SeedInjuryNatures();
                await SeedAccidentSeverities();
                await SeedAccidentStatuses();
                await SeedSampleAccidentFIR();

                _logger.LogInformation("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        private async Task SeedInjuryCategories()
        {
            try
            {
                var existingCount = await _context.InjuryCategories.CountAsync();
                if (existingCount > 0)
                {
                    _logger.LogInformation("Injury Categories already exist ({Count} records). Skipping seed.", existingCount);
                    return;
                }

                var categories = new List<InjuryCategory>
                {
                    new InjuryCategory("Contusion/Bruise", "Blunt force injury without laceration"),
                    new InjuryCategory("Laceration", "Tearing of skin or tissue"),
                    new InjuryCategory("Fracture", "Break or crack in bone"),
                    new InjuryCategory("Burn", "Injury from heat or chemicals"),
                    new InjuryCategory("Strain/Sprain", "Overstretching of muscles or ligaments")
                };

                _logger.LogInformation("Adding {Count} injury categories to database", categories.Count);
                await _context.InjuryCategories.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully seeded {Count} injury category records", categories.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding injury categories");
                throw;
            }
        }

        private async Task SeedInjuryNatures()
        {
            try
            {
                var existingCount = await _context.NaturesOfInjury.CountAsync();
                if (existingCount > 0)
                {
                    _logger.LogInformation("Injury Natures already exist ({Count} records). Skipping seed.", existingCount);
                    return;
                }

                var natures = new List<InjuryNature>
                {
                    new InjuryNature("Abrasion", "Surface rubbing of skin"),
                    new InjuryNature("Amputation", "Loss of body part"),
                    new InjuryNature("Asphyxiation", "Lack of oxygen"),
                    new InjuryNature("Hernia", "Internal part protrudes"),
                    new InjuryNature("Multiple Injuries", "More than one type of injury")
                };

                _logger.LogInformation("Adding {Count} injury natures to database", natures.Count);
                await _context.DomainInjuryNatures.AddRangeAsync(natures);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully seeded {Count} injury nature records", natures.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding injury natures");
                throw;
            }
        }

        private async Task SeedAccidentSeverities()
        {
            try
            {
                var existingCount = await _context.AccidentSeverities.CountAsync();
                if (existingCount > 0)
                {
                    _logger.LogInformation("Accident Severities already exist ({Count} records). Skipping seed.", existingCount);
                    return;
                }

                var severities = new List<AccidentSeverity>
                {
                    new AccidentSeverity("1", "Minor", "No lost work time or first aid only"),
                    new AccidentSeverity("2", "Moderate", "Lost work time injuries"),
                    new AccidentSeverity("3", "Serious", "Serious injuries requiring hospitalization"),
                    new AccidentSeverity("4", "Critical", "Critical injuries or fatalities")
                };

                _logger.LogInformation("Adding {Count} accident severities to database", severities.Count);
                await _context.AccidentSeverities.AddRangeAsync(severities);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully seeded {Count} accident severity records", severities.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding accident severities");
                throw;
            }
        }

        private async Task SeedAccidentStatuses()
        {
            try
            {
                var existingCount = await _context.AccidentStatuses.CountAsync();
                if (existingCount > 0)
                {
                    _logger.LogInformation("Accident Statuses already exist ({Count} records). Skipping seed.", existingCount);
                    return;
                }

                var statuses = new List<AccidentStatus>
                {
                    new AccidentStatus("RPT", "Reported", "Accident reported and under investigation"),
                    new AccidentStatus("INV", "Under Investigation", "Investigation in progress"),
                    new AccidentStatus("CLS", "Closed", "Investigation completed and case closed"),
                    new AccidentStatus("PND", "Pending Follow-up", "Awaiting follow-up action")
                };

                _logger.LogInformation("Adding {Count} accident statuses to database", statuses.Count);
                await _context.AccidentStatuses.AddRangeAsync(statuses);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully seeded {Count} accident status records", statuses.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding accident statuses");
                throw;
            }
        }

        private async Task SeedSampleAccidentFIR()
        {
            try
            {
                var existingCount = await _context.DailyAccidentFIRs.CountAsync();
                if (existingCount > 0)
                {
                    _logger.LogInformation("Daily Accident FIRs already exist ({Count} records). Skipping seed.", existingCount);
                    return;
                }

                var firs = new List<DailyAccidentFIR>
                {
                    new DailyAccidentFIR
                    {
                        AccidentNumber = 1001,
                        EmployeeNumber = "EMP001",
                        EmployeeName = "Michael Johnson",
                        EmployeeDepartment = "Operations",
                        AccidentDateTime = DateTime.UtcNow.AddDays(-5),
                        AccidentLocation = "Warehouse Area A",
                        NatureOfInjury = "Cut on hand",
                        BodyPartAffected = "Right Hand",
                        ShiftName = "Morning",
                        MedicalCentreName = "Clinic",
                        TreatmentGiven = "First Aid",
                        MedicalCentreReceivingDate = DateTime.UtcNow.AddDays(-5),
                        CompanyCode = "COM",
                        EnteredUserID = "USR",
                        EnteredUserNumber = 1001,
                        EnteredDate = DateTime.UtcNow.AddDays(-5),
                        InjuryCategoryCode = 1,
                        NatureOfInjuryCode = 1,
                        PreventiveMeasures = "Wear gloves",
                        CauseOfIncident = "Sharp object",
                        Remarks = "Minor incident"
                    },
                    new DailyAccidentFIR
                    {
                        AccidentNumber = 1002,
                        EmployeeNumber = "EMP002",
                        EmployeeName = "Sarah Williams",
                        EmployeeDepartment = "Manufacturing",
                        AccidentDateTime = DateTime.UtcNow.AddDays(-3),
                        AccidentLocation = "Assembly Line 2",
                        NatureOfInjury = "Back strain",
                        BodyPartAffected = "Back",
                        ShiftName = "Evening",
                        MedicalCentreName = "Hospital",
                        TreatmentGiven = "Rest",
                        MedicalCentreReceivingDate = DateTime.UtcNow.AddDays(-3),
                        CompanyCode = "COM",
                        EnteredUserID = "USR",
                        EnteredUserNumber = 1002,
                        EnteredDate = DateTime.UtcNow.AddDays(-3),
                        InjuryCategoryCode = 5,
                        NatureOfInjuryCode = 2,
                        PreventiveMeasures = "Lift properly",
                        CauseOfIncident = "Heavy load",
                        Remarks = "2 days leave"
                    },
                    new DailyAccidentFIR
                    {
                        AccidentNumber = 1003,
                        ContractorId = "CON",
                        ContractorName = "Alpha Safety",
                        WorkerName = "David Martinez",
                        EmployeeDepartment = "Maintenance",
                        AccidentDateTime = DateTime.UtcNow.AddDays(-1),
                        AccidentLocation = "Electrical Room",
                        NatureOfInjury = "Minor burn",
                        BodyPartAffected = "Forearm",
                        ShiftName = "Day",
                        MedicalCentreName = "Clinic",
                        TreatmentGiven = "Ointment",
                        MedicalCentreReceivingDate = DateTime.UtcNow.AddDays(-1),
                        CompanyCode = "COM",
                        EnteredUserID = "USR",
                        EnteredUserNumber = 1003,
                        EnteredDate = DateTime.UtcNow.AddDays(-1),
                        InjuryCategoryCode = 4,
                        NatureOfInjuryCode = 3,
                        PreventiveMeasures = "PPE training",
                        CauseOfIncident = "Faulty wire",
                        ShiftInChargePersonName = "James A",
                        Remarks = "Reported"
                    }
                };

                _logger.LogInformation("Adding {Count} accident FIRs to database", firs.Count);
                await _context.DailyAccidentFIRs.AddRangeAsync(firs);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully seeded {Count} accident FIR records", firs.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding accident FIRs");
                throw;
            }
        }
    }
}
