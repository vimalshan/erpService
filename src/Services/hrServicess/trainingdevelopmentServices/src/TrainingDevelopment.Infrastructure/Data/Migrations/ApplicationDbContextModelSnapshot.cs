using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrainingDevelopment.Infrastructure.Data;

#nullable disable

namespace TrainingDevelopment.Infrastructure.Data.Migrations;

[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.5")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("TrainingDevelopment.Domain.Entities.TrainingDetail", b =>
        {
            b.Property<decimal>("Id").HasColumnType("decimal(38,0)").HasColumnName("TR_ID");
            b.HasKey("Id");
            b.ToTable("TRAINING_DET");
        });

        modelBuilder.Entity("TrainingDevelopment.Domain.Entities.InstituteMaster", b =>
        {
            b.Property<decimal>("InstituteCode").HasColumnType("decimal(22,0)").HasColumnName("INSTITUTE_CODE");
            b.HasKey("InstituteCode");
            b.ToTable("INSTITUTE_MASTER");
        });

        modelBuilder.Entity("TrainingDevelopment.Domain.Entities.ProgramLovMaster", b =>
        {
            b.Property<string>("TypeCode").HasMaxLength(20).HasColumnName("PRLOV_TYPECODE");
            b.HasKey("TypeCode");
            b.ToTable("PROGRAMLOV_MAST");
        });
#pragma warning restore 612, 618
    }
}
