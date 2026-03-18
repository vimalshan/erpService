using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompensationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CompensationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CompensationService.Domain.Entities.CompensationGrade", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("GRADE_ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CREATED_BY");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("CREATED_ON")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime?>("EffectiveTo")
                        .HasColumnType("date")
                        .HasColumnName("EFFECTIVE_TO");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("UPDATED_BY");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("UPDATED_ON");

                    b.HasKey("Id");

                    b.ToTable("COMP_GRADE", (string)null);

                    b.OwnsOne("CompensationService.Domain.ValueObjects.GradeCode", "GradeCode", b1 =>
                    {
                        b1.Property<long>("CompensationGradeId")
                            .HasColumnType("bigint");

                        b1.Property<string>("Value")
                            .IsRequired()
                            .HasMaxLength(50)
                            .HasColumnType("nvarchar(50)")
                            .HasColumnName("GRADE_CODE");

                        b1.HasKey("CompensationGradeId");

                        b1.HasIndex("Value")
                            .IsUnique();

                        b1.ToTable("COMP_GRADE");

                        b1.WithOwner()
                            .HasForeignKey("CompensationGradeId");
                    });

                    b.OwnsOne("CompensationService.Domain.ValueObjects.SalaryStructure", "SalaryStructure", b1 =>
                    {
                        b1.Property<long>("CompensationGradeId")
                            .HasColumnType("bigint");

                        b1.Property<decimal>("BaseSalary")
                            .HasPrecision(19, 2)
                            .HasColumnType("numeric(19,2)")
                            .HasColumnName("BASE_SALARY");

                        b1.Property<decimal>("DaPercentage")
                            .HasPrecision(5, 2)
                            .HasColumnType("numeric(5,2)")
                            .HasColumnName("DA_PERCENTAGE");

                        b1.Property<decimal>("HraPercentage")
                            .HasPrecision(5, 2)
                            .HasColumnType("numeric(5,2)")
                            .HasColumnName("HRA_PERCENTAGE");

                        b1.HasKey("CompensationGradeId");

                        b1.ToTable("COMP_GRADE");

                        b1.WithOwner()
                            .HasForeignKey("CompensationGradeId");
                    });

                    b.OwnsOne("CompensationService.Domain.ValueObjects.GradeStatus", "Status", b1 =>
                    {
                        b1.Property<long>("CompensationGradeId")
                            .HasColumnType("bigint");

                        b1.Property<string>("Value")
                            .IsRequired()
                            .HasMaxLength(1)
                            .HasColumnType("nvarchar(1)")
                            .HasColumnName("GRADE_STATUS");

                        b1.HasKey("CompensationGradeId");

                        b1.ToTable("COMP_GRADE");

                        b1.WithOwner()
                            .HasForeignKey("CompensationGradeId");
                    });

                    b.Property<int>("GradeLevel")
                        .HasColumnType("int")
                        .HasColumnName("GRADE_LEVEL");

                    b.Property<string>("GradeName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("GRADE_NAME");

                    b.Property<DateTime>("EffectiveFrom")
                        .HasColumnType("date")
                        .HasColumnName("EFFECTIVE_FROM");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("int");
                });
#pragma warning restore 612, 618
        }
    }
}
