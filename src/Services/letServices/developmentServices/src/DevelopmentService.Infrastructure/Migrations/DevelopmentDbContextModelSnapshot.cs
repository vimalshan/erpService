using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using DevelopmentService.Infrastructure.Data;

#nullable disable

namespace DevelopmentService.Infrastructure.Migrations;

[DbContext(typeof(DevelopmentDbContext))]
partial class DevelopmentDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("DevelopmentService.Domain.Entities.LetPlan", b =>
        {
            b.Property<long>("ReqNum").HasColumnName("DD_REQNUM").ValueGeneratedNever();
            b.Property<long?>("Sno").HasColumnName("DD_SNO");
            b.Property<string>("UserId").HasColumnName("DD_USERID").HasMaxLength(255);
            b.Property<long?>("PinNum").HasColumnName("DD_PINNUM");
            b.Property<string>("DevSource").HasColumnName("DD_DEVSOURCE").HasMaxLength(255);
            b.Property<string>("DevNeed").HasColumnName("DD_DEVNEED").HasMaxLength(255);
            b.Property<string>("DevIndicator").HasColumnName("DD_DEVINDICATOR").HasMaxLength(255);
            b.Property<long?>("DevMode").HasColumnName("DD_DEVMODE");
            b.Property<string>("RecProg").HasColumnName("DD_RECPROG").HasMaxLength(255);
            b.Property<string>("TrainingProgram").HasColumnName("DD_TRAININGPROGRAM").HasMaxLength(255);
            b.Property<long?>("InternalTraining").HasColumnName("DD_INTERNALTRAINING");
            b.Property<string>("RevDate").HasColumnName("DD_REVDATE").HasMaxLength(255);
            b.Property<long?>("Priority").HasColumnName("DD_PRIORITY");
            b.Property<DateTime?>("EntDate").HasColumnName("DD_ENTDATE").HasColumnType("datetime2(3)");
            b.Property<string>("AppStatus").HasColumnName("DD_APPSTATUS").HasMaxLength(1);
            b.Property<string>("BhrStatus").HasColumnName("DD_BHRSTATUS").HasMaxLength(1);
            b.HasKey("ReqNum");
            b.ToTable("DD_LETPLAN");
        });
#pragma warning restore 612, 618
    }
}
