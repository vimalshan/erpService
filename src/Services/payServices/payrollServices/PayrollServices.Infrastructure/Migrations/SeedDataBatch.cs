using PayrollServices.Domain.Entities;
using PayrollServices.Infrastructure.Data;

namespace PayrollServices.Infrastructure.Migrations;

/// <summary>
/// Database seed data functionality
/// </summary>
public class SeedDataBatch
{
    public static async Task SeedAsync(PayrollDbContext context)
    {
        try
        {
            // Seed Payroll Batches
            if (!context.PayrollBatches.Any())
            {
                var batches = GetSeedBatches();
                context.PayrollBatches.AddRange(batches);
                await context.SaveChangesAsync();
                Console.WriteLine($"Seeded {batches.Count} payroll batches");
            }

            // Seed Payroll Transactions
            if (!context.PayrollTransactions.Any())
            {
                var transactions = GetSeedTransactions();
                context.PayrollTransactions.AddRange(transactions);
                await context.SaveChangesAsync();
                Console.WriteLine($"Seeded {transactions.Count} payroll transactions");
            }

            // Seed Payroll Adjustments
            if (!context.PayrollAdjustments.Any())
            {
                var adjustments = GetSeedAdjustments();
                context.PayrollAdjustments.AddRange(adjustments);
                await context.SaveChangesAsync();
                Console.WriteLine($"Seeded {adjustments.Count} payroll adjustments");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while seeding database: {ex.Message}");
            throw;
        }
    }

    private static List<PayrollBatch> GetSeedBatches()
    {
        return new List<PayrollBatch>
        {
            new()
            {
                BatchId = 1,
                BatchMonth = "2024-01",
                Status = BatchStatus.Completed,
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 1, 1, 10, 0, 0),
                UpdatedOn = new DateTime(2024, 1, 5, 17, 30, 0),
                UpdatedBy = 1
            },
            new()
            {
                BatchId = 2,
                BatchMonth = "2024-02",
                Status = BatchStatus.Completed,
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 2, 1, 10, 0, 0),
                UpdatedOn = new DateTime(2024, 2, 5, 17, 30, 0),
                UpdatedBy = 1
            },
            new()
            {
                BatchId = 3,
                BatchMonth = "2024-03",
                Status = BatchStatus.Processing,
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 3, 1, 10, 0, 0)
            }
        };
    }

    private static List<PayrollTransaction> GetSeedTransactions()
    {
        return new List<PayrollTransaction>
        {
            // Batch 1 - January 2024
            new()
            {
                TransactionId = 1,
                EmployeeSystemId = 101,
                BatchId = 1,
                Month = "2024-01",
                GrossSalary = 55000,
                Deductions = 5500,
                NetSalary = 49500,
                Status = TransactionStatus.Disbursed,
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 1, 2, 08, 0, 0),
                UpdatedOn = new DateTime(2024, 1, 5, 16, 0, 0),
                UpdatedBy = 1
            },
            new()
            {
                TransactionId = 2,
                EmployeeSystemId = 102,
                BatchId = 1,
                Month = "2024-01",
                GrossSalary = 60000,
                Deductions = 6000,
                NetSalary = 54000,
                Status = TransactionStatus.Disbursed,
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 1, 2, 08, 0, 0),
                UpdatedOn = new DateTime(2024, 1, 5, 16, 0, 0),
                UpdatedBy = 1
            },
            new()
            {
                TransactionId = 3,
                EmployeeSystemId = 103,
                BatchId = 1,
                Month = "2024-01",
                GrossSalary = 50000,
                Deductions = 5000,
                NetSalary = 45000,
                Status = TransactionStatus.Disbursed,
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 1, 2, 08, 0, 0),
                UpdatedOn = new DateTime(2024, 1, 5, 16, 0, 0),
                UpdatedBy = 1
            },

            // Batch 2 - February 2024
            new()
            {
                TransactionId = 4,
                EmployeeSystemId = 101,
                BatchId = 2,
                Month = "2024-02",
                GrossSalary = 55000,
                Deductions = 5500,
                NetSalary = 49500,
                Status = TransactionStatus.Disbursed,
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 2, 2, 08, 0, 0),
                UpdatedOn = new DateTime(2024, 2, 5, 16, 0, 0),
                UpdatedBy = 1
            },
            new()
            {
                TransactionId = 5,
                EmployeeSystemId = 102,
                BatchId = 2,
                Month = "2024-02",
                GrossSalary = 60000,
                Deductions = 6000,
                NetSalary = 54000,
                Status = TransactionStatus.Disbursed,
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 2, 2, 08, 0, 0),
                UpdatedOn = new DateTime(2024, 2, 5, 16, 0, 0),
                UpdatedBy = 1
            }
        };
    }

    private static List<PayrollAdjustment> GetSeedAdjustments()
    {
        return new List<PayrollAdjustment>
        {
            // Allowances
            new()
            {
                AdjustmentId = 1,
                EmployeeSystemId = 101,
                Amount = 2000,
                AdjustmentType = AdjustmentType.Allowance,
                AdjustmentDate = new DateTime(2024, 1, 1),
                Description = "Performance Bonus",
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 1, 1, 09, 0, 0),
                ApprovedOn = new DateTime(2024, 1, 1, 10, 0, 0),
                ApprovedBy = 1
            },
            new()
            {
                AdjustmentId = 2,
                EmployeeSystemId = 102,
                Amount = 1500,
                AdjustmentType = AdjustmentType.Allowance,
                AdjustmentDate = new DateTime(2024, 1, 1),
                Description = "HRA",
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 1, 1, 09, 0, 0),
                ApprovedOn = new DateTime(2024, 1, 1, 10, 0, 0),
                ApprovedBy = 1
            },

            // Deductions
            new()
            {
                AdjustmentId = 3,
                EmployeeSystemId = 101,
                Amount = 1000,
                AdjustmentType = AdjustmentType.Deduction,
                AdjustmentDate = new DateTime(2024, 1, 5),
                Description = "Loan EMI",
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 1, 5, 14, 0, 0),
                ApprovedOn = new DateTime(2024, 1, 5, 15, 0, 0),
                ApprovedBy = 1
            },
            new()
            {
                AdjustmentId = 4,
                EmployeeSystemId = 103,
                Amount = 500,
                AdjustmentType = AdjustmentType.Deduction,
                AdjustmentDate = new DateTime(2024, 1, 5),
                Description = "Canteen Charges",
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 1, 5, 14, 0, 0),
                ApprovedOn = new DateTime(2024, 1, 5, 15, 0, 0),
                ApprovedBy = 1
            },

            // Arrears
            new()
            {
                AdjustmentId = 5,
                EmployeeSystemId = 102,
                Amount = 1200,
                AdjustmentType = AdjustmentType.Arrear,
                AdjustmentDate = new DateTime(2024, 1, 10),
                Description = "Previous Month Arrear",
                CreatedBy = 1,
                CreatedOn = new DateTime(2024, 1, 10, 11, 0, 0),
                ApprovedOn = new DateTime(2024, 1, 10, 12, 0, 0),
                ApprovedBy = 1
            }
        };
    }
}
