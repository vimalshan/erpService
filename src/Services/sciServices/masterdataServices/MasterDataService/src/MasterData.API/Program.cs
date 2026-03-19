using MasterData.API;
using MasterData.Infrastructure;
using MasterData.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MasterDataDbContext>();
    await context.Database.MigrateAsync();
    await DataSeeder.SeedDataAsync(context);
}

app.ConfigurePipeline();

app.Run();
