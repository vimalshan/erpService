using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using UtilityService.Infrastructure.Data;

#nullable disable

namespace UtilityService.Infrastructure.Migrations;

[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.3")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("UtilityService.Domain.Entities.ToadPlanSql", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int")
                .HasColumnName("ID");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<DateTime>("CreatedAt")
                .ValueGeneratedOnAdd()
                .HasColumnType("datetime2")
                .HasColumnName("CREATED_AT")
                .HasDefaultValueSql("GETUTCDATE()");

            b.Property<bool>("IsDeleted")
                .ValueGeneratedOnAdd()
                .HasColumnType("bit")
                .HasColumnName("IS_DELETED")
                .HasDefaultValue(false);

            b.Property<string>("Statement")
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnType("varchar(2000)")
                .HasColumnName("STATEMENT");

            b.Property<string>("StatementId")
                .IsRequired()
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnType("varchar(32)")
                .HasColumnName("STATEMENT_ID");

            b.Property<DateTime?>("Timestamp")
                .HasColumnType("datetime2(3)")
                .HasColumnName("TIMESTAMP");

            b.Property<DateTime?>("UpdatedAt")
                .HasColumnType("datetime2")
                .HasColumnName("UPDATED_AT");

            b.Property<string>("Username")
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnType("varchar(30)")
                .HasColumnName("USERNAME");

            b.HasKey("Id");

            b.HasIndex("StatementId").HasDatabaseName("IX_TOAD_PLAN_SQL_STATEMENT_ID");
            b.HasIndex("Username").HasDatabaseName("IX_TOAD_PLAN_SQL_USERNAME");

            b.ToTable("TOAD_PLAN_SQL");
        });
#pragma warning restore 612, 618
    }
}
