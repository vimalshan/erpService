using LocationService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace LocationService.Infrastructure.Persistence.Seeds
{
    /// <summary>
    /// Database seed data for initial setup
    /// </summary>
    public static class SeedData
    {
        public static async Task SeedDatabaseAsync(LocationServiceDbContext context)
        {
            try
            {
                // Seed Locations
                if (!context.Locations.Any())
                {
                    var locations = new List<LocationAggregate>
                    {
                        new LocationAggregate(
                            locationCode: "LOC-001",
                            locationName: "Main Office - Delhi",
                            createdBy: 1,
                            streetAddress: "123 Business Street",
                            city: "New Delhi",
                            state: "Delhi",
                            postalCode: "110001",
                            country: "India",
                            phone: "+91-11-4000-0000",
                            email: "delhi@company.com",
                            contactPerson: "Rajesh Kumar"),

                        new LocationAggregate(
                            locationCode: "LOC-002",
                            locationName: "Regional Office - Mumbai",
                            createdBy: 1,
                            streetAddress: "456 Corporate Ave",
                            city: "Mumbai",
                            state: "Maharashtra",
                            postalCode: "400001",
                            country: "India",
                            phone: "+91-22-5000-0000",
                            email: "mumbai@company.com",
                            contactPerson: "Priya Singh"),

                        new LocationAggregate(
                            locationCode: "LOC-003",
                            locationName: "Tech Hub - Bangalore",
                            createdBy: 1,
                            streetAddress: "789 Tech Park",
                            city: "Bangalore",
                            state: "Karnataka",
                            postalCode: "560001",
                            country: "India",
                            phone: "+91-80-6000-0000",
                            email: "bangalore@company.com",
                            contactPerson: "Amit Patel")
                    };

                    await context.Locations.AddRangeAsync(locations);
                    await context.SaveChangesAsync();
                }

                // Seed Rooms
                if (!context.Rooms.Any())
                {
                    var locations = await context.Locations.ToListAsync();
                    var mainLocation = locations.FirstOrDefault(l => l.LocationCode == "LOC-001");

                    if (mainLocation != null)
                    {
                        var rooms = new List<RoomAggregate>
                        {
                            new RoomAggregate(
                                locationId: mainLocation.Id,
                                roomCode: "ROOM-101",
                                roomName: "Board Room",
                                createdBy: 1,
                                roomCapacity: 30,
                                roomType: "CONFERENCE",
                                floorNumber: 1),

                            new RoomAggregate(
                                locationId: mainLocation.Id,
                                roomCode: "ROOM-102",
                                roomName: "Training Hall",
                                createdBy: 1,
                                roomCapacity: 50,
                                roomType: "TRAINING",
                                floorNumber: 2),

                            new RoomAggregate(
                                locationId: mainLocation.Id,
                                roomCode: "ROOM-103",
                                roomName: "Meeting Room A",
                                createdBy: 1,
                                roomCapacity: 10,
                                roomType: "MEETING",
                                floorNumber: 1)
                        };

                        await context.Rooms.AddRangeAsync(rooms);
                        await context.SaveChangesAsync();
                    }
                }

                // Seed Room Resources
                if (!context.RoomResources.Any())
                {
                    var mainLocation = await context.Locations
                        .AsNoTracking()
                        .FirstOrDefaultAsync(l => l.LocationCode == "LOC-001");

                    if (mainLocation != null)
                    {
                        var boardRoom = await context.Rooms
                            .AsNoTracking()
                            .FirstOrDefaultAsync(r => r.LocationId == mainLocation.Id && r.RoomCode == "ROOM-101");

                        if (boardRoom != null)
                        {
                            var resources = new List<RoomResourceAggregate>
                            {
                                new RoomResourceAggregate(
                                    roomId: boardRoom.Id,
                                    locationId: mainLocation.Id,
                                    resourceCode: "PROJ-001",
                                    resourceName: "Projector - Sony",
                                    createdBy: 1,
                                    resourceType: "PROJECTOR",
                                    resourceQuantity: 1),

                                new RoomResourceAggregate(
                                    roomId: boardRoom.Id,
                                    locationId: mainLocation.Id,
                                    resourceCode: "WB-001",
                                    resourceName: "Whiteboard Interactive",
                                    createdBy: 1,
                                    resourceType: "WHITEBOARD",
                                    resourceQuantity: 1),

                                new RoomResourceAggregate(
                                    roomId: boardRoom.Id,
                                    locationId: mainLocation.Id,
                                    resourceCode: "MIC-001",
                                    resourceName: "Wireless Microphone",
                                    createdBy: 1,
                                    resourceType: "MICROPHONE",
                                    resourceQuantity: 2)
                            };

                            await context.RoomResources.AddRangeAsync(resources);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding database: {ex.Message}");
                throw;
            }
        }
    }
}
