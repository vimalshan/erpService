using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingDevelopment.Infrastructure.Data.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20260314000001_InitialCreate")]
partial class InitialCreate
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.5")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("TrainingDevelopment.Domain.Entities.TrainingDetail", b =>
        {
            b.Property<decimal>("Id").HasColumnType("decimal(38,0)").HasColumnName("TR_ID");
            b.Property<decimal>("FinancialYear").HasColumnType("decimal(38,0)").HasColumnName("TR_FINYEAR");
            b.Property<decimal>("EmployeeSysId").HasColumnType("decimal(38,0)").HasColumnName("TR_EMPSYSID");
            b.Property<string>("TrainingNeed").IsRequired().HasMaxLength(1000).HasColumnName("TR_NEED");
            b.Property<string>("GapArea").IsRequired().HasMaxLength(1000).HasColumnName("TR_GAPS");
            b.Property<decimal>("Mode").HasColumnType("decimal(38,0)").HasColumnName("TR_MODE");
            b.Property<decimal>("ProgramId").HasColumnType("decimal(38,0)").HasColumnName("TR_PROGRAMID");
            b.Property<string>("ProgramDescription").IsRequired().HasMaxLength(1000).HasColumnName("TR_PROGRAMDESC");
            b.Property<DateTime>("PlannedFrom").HasColumnType("datetime2(3)").HasColumnName("TR_PLANFROM");
            b.Property<DateTime>("PlannedTo").HasColumnType("datetime2(3)").HasColumnName("TR_PLANTO");
            b.Property<string>("Status").IsRequired().HasMaxLength(1).HasColumnName("TR_STATUS");
            b.Property<DateTime?>("ActualFrom").HasColumnType("datetime2(3)").HasColumnName("TR_ACTFROM");
            b.Property<DateTime?>("ActualTo").HasColumnType("datetime2(3)").HasColumnName("TR_ACTTO");
            b.Property<decimal?>("InstituteId").HasColumnType("decimal(38,0)").HasColumnName("TR_INSTITUTEID");
            b.Property<string?>("InstituteDescription").HasMaxLength(1000).HasColumnName("TR_INSTITUTEDESC");
            b.Property<decimal?>("TrainerId").HasColumnType("decimal(38,0)").HasColumnName("TR_TRAINERID");
            b.Property<string?>("TrainerDescription").HasMaxLength(65).HasColumnName("TR_TRAINERDESC");
            b.Property<decimal?>("PlaceId").HasColumnType("decimal(38,0)").HasColumnName("TR_PLACEID");
            b.Property<string?>("Place").HasMaxLength(65).HasColumnName("TR_PLACE");
            b.Property<decimal?>("Cost").HasColumnType("decimal(38,0)").HasColumnName("TR_COST");
            b.Property<string?>("DroppedRemarks").HasMaxLength(1000).HasColumnName("TR_DROPREMARKS");
            b.Property<decimal?>("LastModifiedBy").HasColumnType("decimal(22,0)").HasColumnName("TR_LASTMODIFIEDBY");
            b.Property<DateTime?>("LastModifiedOn").HasColumnType("datetime2(3)").HasColumnName("TR_LASTMODIFIEDON");
            b.HasKey("Id");
            b.ToTable("TRAINING_DET");
        });

        modelBuilder.Entity("TrainingDevelopment.Domain.Entities.InstituteMaster", b =>
        {
            b.Property<decimal>("InstituteCode").HasColumnType("decimal(22,0)").HasColumnName("INSTITUTE_CODE");
            b.Property<string?>("InstituteName").HasMaxLength(100).HasColumnName("INSTITUTE_NAME");
            b.Property<string?>("Address1").HasMaxLength(100).HasColumnName("INSTITUTE_ADD1");
            b.Property<string?>("Address2").HasMaxLength(100).HasColumnName("INSTITUTE_ADD2");
            b.Property<string?>("City").HasMaxLength(50).HasColumnName("INSTITUTE_CITY");
            b.Property<string?>("State").HasMaxLength(50).HasColumnName("INSTITUTE_STATE");
            b.Property<string?>("Pin").HasMaxLength(50).HasColumnName("INSTITUTE_PIN");
            b.Property<string?>("Phone").HasMaxLength(50).HasColumnName("INSTITUTE_PHONE");
            b.Property<string?>("Fax").HasMaxLength(50).HasColumnName("INSTITUTE_FAX");
            b.Property<string?>("Email").HasMaxLength(50).HasColumnName("INSTITUTE_EMAIL");
            b.Property<string?>("Url").HasMaxLength(50).HasColumnName("INSTITUTE_URL");
            b.Property<string?>("InstituteType").HasMaxLength(50).HasColumnName("INSTITUTE_TYPE");
            b.Property<string>("CampusRecruit").HasMaxLength(1).HasColumnName("INSTITUTE_CAMPUSRECRUIT");
            b.Property<string?>("InstituteClass").HasMaxLength(3).HasColumnName("INSTITUTE_CLASS");
            b.Property<decimal?>("LastModifiedBy").HasColumnType("decimal(22,0)").HasColumnName("INSTITUTE_MODIFIEDBY");
            b.Property<DateTime?>("LastModifiedOn").HasColumnType("datetime2(3)").HasColumnName("INSTITUTE_MODIFIEDON");
            b.HasKey("InstituteCode");
            b.ToTable("INSTITUTE_MASTER");
        });

        modelBuilder.Entity("TrainingDevelopment.Domain.Entities.ProgramLovMaster", b =>
        {
            b.Property<string>("TypeCode").HasMaxLength(20).HasColumnName("PRLOV_TYPECODE");
            b.Property<string>("Code").IsRequired().HasMaxLength(5).HasColumnName("PRLOV_CODE");
            b.Property<string>("Name").IsRequired().HasMaxLength(200).HasColumnName("PRLOV_NAME");
            b.HasKey("TypeCode");
            b.ToTable("PROGRAMLOV_MAST");
        });
#pragma warning restore 612, 618
    }
}
