using Microsoft.EntityFrameworkCore;
using TaxService.Domain.Entities;
using TaxService.Domain.ValueObjects;

namespace TaxService.Infrastructure.Data;

/// <summary>
/// Seed data for Tax Service database initialization.
/// Contains sample tax rates, payees, exemptions, and deductions.
/// </summary>
public static class TaxServiceDbContextSeed
{
    public static async Task SeedAsync(TaxServiceDbContext context)
    {
        try
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Only seed if ConditionalMasters table is empty
            if (await context.ConditionalMasters.AnyAsync())
            {
                Console.WriteLine("Database already seeded. Skipping seed data...");
                return;
            }

            Console.WriteLine("Seeding database with initial data...");

            // Create sample conditional masters (Payees)
            var samplePayees = CreateSamplePayees();
            if (samplePayees.Any())
            {
                context.ConditionalMasters.AddRange(samplePayees);
                await context.SaveChangesAsync();
                Console.WriteLine($"Seeded {samplePayees.Count} payee records.");
            }

            // Create sample tax details (Employees)
            var sampleTaxDetails = CreateSampleTaxDetails();
            if (sampleTaxDetails.Any())
            {
                context.TaxMarginalDetails.AddRange(sampleTaxDetails);
                await context.SaveChangesAsync();
                Console.WriteLine($"Seeded {sampleTaxDetails.Count} employee tax records.");
            }

            Console.WriteLine("Database seeding completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding database: {ex.Message}");
            throw;
        }
    }

    private static List<ConditionalMaster> CreateSamplePayees()
    {
        var financialYear = DateTime.UtcNow.Year;
        var payees = new List<ConditionalMaster>();

        try
        {
            // Payee 1: ABC Corporation
            var payee1 = ConditionalMaster.Create(
                payeeId: "PAY001",
                payeeName: "ABC Corporation Ltd.",
                payeeAddress: "123 Business Park, New Delhi, 110001",
                payeePAN: "AAAA0001K",
                taxRegime: "Old",
                financialYear: financialYear,
                createdBy: "admin"
            );
            payees.Add(payee1);

            // Payee 2: XYZ Industries
            var payee2 = ConditionalMaster.Create(
                payeeId: "PAY002",
                payeeName: "XYZ Industries Pvt. Ltd.",
                payeeAddress: "456 Industrial Area, Mumbai, 400016",
                payeePAN: "BBBB0002K",
                taxRegime: "New",
                financialYear: financialYear,
                createdBy: "admin"
            );
            payees.Add(payee2);

            // Payee 3: Global Tech Solutions
            var payee3 = ConditionalMaster.Create(
                payeeId: "PAY003",
                payeeName: "Global Tech Solutions Pvt. Ltd.",
                payeeAddress: "789 Tech Park, Bangalore, 560001",
                payeePAN: "CCCC0003K",
                taxRegime: "Old",
                financialYear: financialYear,
                createdBy: "admin"
            );
            payees.Add(payee3);

            return payees;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating sample payees: {ex.Message}");
            return new List<ConditionalMaster>();
        }
    }

    private static List<TaxMarginalDetail> CreateSampleTaxDetails()
    {
        var financialYear = DateTime.UtcNow.Year;
        var taxDetails = new List<TaxMarginalDetail>();

        try
        {
            var taxRates = GetSampleTaxRates();

            // Employee 1
            var employee1 = TaxMarginalDetail.Create(
                employeeSystemId: 1001L,
                financialYear: financialYear,
                grossIncome: new Money(900000, "INR"),
                standardDeduction: new Money(50000, "INR"),
                createdBy: "admin"
            );
            employee1.CalculateTax(taxRates);
            taxDetails.Add(employee1);

            // Employee 2
            var employee2 = TaxMarginalDetail.Create(
                employeeSystemId: 1002L,
                financialYear: financialYear,
                grossIncome: new Money(1500000, "INR"),
                standardDeduction: new Money(50000, "INR"),
                createdBy: "admin"
            );
            employee2.CalculateTax(taxRates);
            taxDetails.Add(employee2);

            // Employee 3
            var employee3 = TaxMarginalDetail.Create(
                employeeSystemId: 1003L,
                financialYear: financialYear,
                grossIncome: new Money(500000, "INR"),
                standardDeduction: new Money(50000, "INR"),
                createdBy: "admin"
            );
            employee3.CalculateTax(taxRates);
            taxDetails.Add(employee3);

            // Employee 4
            var employee4 = TaxMarginalDetail.Create(
                employeeSystemId: 1004L,
                financialYear: financialYear,
                grossIncome: new Money(2000000, "INR"),
                standardDeduction: new Money(50000, "INR"),
                createdBy: "admin"
            );
            employee4.CalculateTax(taxRates);
            taxDetails.Add(employee4);

            return taxDetails;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating sample tax details: {ex.Message}");
            return new List<TaxMarginalDetail>();
        }
    }

    private static List<TaxRate> GetSampleTaxRates()
    {
        // Indian Tax Slabs for FY 2024-25 (Old Regime)
        // Note: TaxRate is from Domain.ValueObjects
        return new List<TaxRate>
        {
            new TaxRate(0m, 0, 250000),           // 0% - Up to 2.5L
            new TaxRate(5m, 250000, 500000),     // 5% - 2.5L to 5L
            new TaxRate(20m, 500000, 1000000),   // 20% - 5L to 10L
            new TaxRate(30m, 1000000, decimal.MaxValue) // 30% - Above 10L
        };
    }
}
