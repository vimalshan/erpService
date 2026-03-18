using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ReportingService.Infrastructure.Data;

#nullable disable

namespace ReportingService.Infrastructure.Migrations
{
    [DbContext(typeof(ReportingDbContext))]
    partial class ReportingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            // Model configuration
            // Base configurations for all entities
#pragma warning restore 612, 618
        }
    }
}
