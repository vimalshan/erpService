using AgencyService.Domain.Entities;
using AgencyService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AgencyService.Infrastructure.Data;

public class SeedDataService
{
    private readonly AgencyDbContext _dbContext;

    public SeedDataService(AgencyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync()
    {
        await SeedAirlinesAsync();
        await SeedAgenciesAsync();
        await SeedVendorsAsync();
    }

    private async Task SeedAirlinesAsync()
    {
        if (await _dbContext.Airlines.AnyAsync())
            return;

        var airlines = new[]
        {
            new Airline("AI", "Air India"),
            new Airline("BA", "British Airways"),
            new Airline("LH", "Lufthansa"),
            new Airline("AA", "American Airlines"),
            new Airline("UA", "United Airlines"),
            new Airline("QA", "Qatar Airways"),
            new Airline("EK", "Emirates"),
            new Airline("AF", "Air France")
        };

        _dbContext.Airlines.AddRange(airlines);
        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedAgenciesAsync()
    {
        if (await _dbContext.Agencies.AnyAsync())
            return;

        var agencies = new[]
        {
            new Agency(1001, "Global Travel Solutions",
                AgencyType.Create("Air"),
                new ContactInfo("info@globaltravel.com", "+1-800-123-4567"),
                new Address("123 Travel Street, New York, NY 10001", null, null, null)),
            new Agency(1002, "Express Transport Agency",
                AgencyType.Create("Cab"),
                new ContactInfo("contact@express.com", "+1-800-987-6543"),
                new Address("456 Transport Ave, Los Angeles, CA 90001", null, null, null)),
            new Agency(1003, "Railway Reservations Ltd",
                AgencyType.Create("Train"),
                new ContactInfo("bookings@railways.com", "+1-800-555-0123"),
                new Address("789 Rail Road, Chicago, IL 60601", null, null, null)),
            new Agency(1004, "Bus Tour Services",
                AgencyType.Create("Bus"),
                new ContactInfo("tours@bustrans.com", "+1-800-555-0456"),
                new Address("321 Bus Lane, Houston, TX 77001", null, null, null))
        };

        _dbContext.Agencies.AddRange(agencies);
        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedVendorsAsync()
    {
        if (await _dbContext.Vendors.AnyAsync())
            return;

        var vendors = new[]
        {
            new Vendor(1, "Taj Hotels",          "H", "+91-22-6665-3000", "Apollo Bunder, Mumbai"),
            new Vendor(2, "Marriott Hotels",      "H", "+1-303-571-1000",  "Denver, Colorado"),
            new Vendor(3, "Premier Cabs Services","V", "+1-212-555-0100",  "789 Transportation Blvd, New York"),
            new Vendor(4, "Elite Airlines Catering","V","+1-404-555-0200", "456 Airport Way, Atlanta"),
            new Vendor(5, "Grand Palaces Resorts", "H", "+1-702-731-7110", "Las Vegas, Nevada")
        };

        _dbContext.Vendors.AddRange(vendors);
        await _dbContext.SaveChangesAsync();
    }
}
