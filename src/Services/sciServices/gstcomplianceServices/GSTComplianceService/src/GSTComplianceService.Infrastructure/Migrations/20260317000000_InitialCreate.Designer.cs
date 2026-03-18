
using GSTComplianceService.Domain.Entities;
using GSTComplianceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;

#nullable disable

namespace GSTComplianceService.Infrastructure.Migrations
{
    [DbContext(typeof(GstDbContext))]
    [Migration("20260317000000_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GSTComplianceService.Domain.Entities.GstHsnDetail", b =>
                {
                    b.Property<long>("GstHsnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("GSTHSN_ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("GstHsnId"));

                    b.Property<long>("GstHsnGstId")
                        .HasColumnType("bigint")
                        .HasColumnName("GSTHSN_GSTID");

                    b.Property<string>("GstHsnProductName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GSTHSN_PRODUCTNAME");

                    b.Property<string>("GstHsnCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("GSTHSN_HSNCODE");

                    b.Property<string>("GstHsnRemarks")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("GSTHSN_REMARKS");

                    b.HasKey("GstHsnId");

                    b.HasIndex("GstHsnGstId");

                    b.ToTable("GST_HSNDET", (string)null);
                });

            modelBuilder.Entity("GSTComplianceService.Domain.Entities.GstMain", b =>
                {
                    b.Property<long>("GstId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("GST_ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("GstId"));

                    b.Property<string>("GstContactEmailId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_CONTACTEMAILID");

                    b.Property<string>("GstContactMobileNo")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("GST_CONTACTMOBILENO");

                    b.Property<string>("GstContactName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_CONTACTNAME");

                    b.Property<DateTime>("GstCreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("GST_CREATEDON")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("GstDigitalFlag")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("GST_DIGITALFLAG");

                    b.Property<string>("GstEmailId")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("GST_EMAILID");

                    b.Property<long?>("GstEnteredBy")
                        .HasColumnType("bigint")
                        .HasColumnName("GST_ENTEREDBY");

                    b.Property<string>("GstEnteredByFlag")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)")
                        .HasColumnName("GST_ENTEREDBYFLA");

                    b.Property<string>("GstGstnCopy")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("GST_GSTNCOPY");

                    b.Property<DateTime?>("GstModifiedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("GST_MODIFIEDON");

                    b.Property<string>("GstMobileNo")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("GST_MOBILENO");

                    b.Property<string>("GstPanNo")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("GST_PANNO");

                    b.Property<int>("GstRegistrationType")
                        .HasColumnType("int")
                        .HasColumnName("GST_REGISTRATIONTYPE");

                    b.Property<string>("GstRemarks")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("GST_REMARKS");

                    b.Property<string>("GstScreenType")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)")
                        .HasColumnName("GST_SCREENTYPE");

                    b.Property<string>("GstStatus")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)")
                        .HasColumnName("GST_STATUS");

                    b.Property<string>("GstType")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)")
                        .HasColumnName("GST_TYPE");

                    b.Property<int?>("GstVendConst")
                        .HasColumnType("int")
                        .HasColumnName("GST_VENDCONST");

                    b.Property<string>("GstVendState")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_VENDSTATE");

                    b.Property<string>("GstVendAddFlag")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)")
                        .HasColumnName("GST_VENDADDFLAG");

                    b.Property<string>("GstVendAddLine1")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("GST_VENDADDLINE1");

                    b.Property<string>("GstVendAddLine2")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_VENDADDLINE2");

                    b.Property<string>("GstVendAddLine3")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_VENDADDLINE3");

                    b.Property<string>("GstVendAddLine4")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_VENDADDLINE4");

                    b.Property<string>("GstVendCity")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_VENDCITY");

                    b.Property<string>("GstVendCityName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_VENDCITYNAME");

                    b.Property<long?>("GstVendorId")
                        .HasColumnType("bigint")
                        .HasColumnName("GST_VENDORID");

                    b.Property<string>("GstVendorName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("GST_VENDORNAME");

                    b.Property<string>("GstVendorNameFlag")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)")
                        .HasColumnName("GST_VENDORNAMEFLAG");

                    b.Property<string>("GstVendPincode")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_VENDPINCODE");

                    b.HasKey("GstId");

                    b.HasAlternateKey("GstPanNo");

                    b.ToTable("GST_MAIN", (string)null);
                });

            modelBuilder.Entity("GSTComplianceService.Domain.Entities.GstServiceDetail", b =>
                {
                    b.Property<long>("GstSacId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("GSTSAC_ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("GstSacId"));

                    b.Property<long>("GstSacGstId")
                        .HasColumnType("bigint")
                        .HasColumnName("GSTSAC_GSTID");

                    b.Property<string>("GstSacServiceName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GSTSAC_SERVICENAME");

                    b.Property<string>("GstSacCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("GSTSAC_SACCODE");

                    b.Property<string>("GstSacRemarks")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("GSTSAC_REMARKS");

                    b.HasKey("GstSacId");

                    b.HasIndex("GstSacGstId");

                    b.ToTable("GST_SERVDET", (string)null);
                });

            modelBuilder.Entity("GSTComplianceService.Domain.Entities.GstStateRegDetail", b =>
                {
                    b.Property<long>("GstTinId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("GST_TINID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("GstTinId"));

                    b.Property<string>("GstAddress")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("GST_ADDRESS");

                    b.Property<string>("GstArnCopy")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("GST_ARNCOPY");

                    b.Property<string>("GstArnNo")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("GST_ARNNO");

                    b.Property<string>("GstArnTempFile")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("GST_ARNTEMPFILE");

                    b.Property<string>("GstContactPerson")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_CONTACTPERSON");

                    b.Property<string>("GstEmailId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_EMAILID");

                    b.Property<string>("GstExcNo")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("GST_EXCNO");

                    b.Property<long>("GstId")
                        .HasColumnType("bigint")
                        .HasColumnName("GST_ID");

                    b.Property<string>("GstGstinNo")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("GST_GSTINNO");

                    b.Property<string>("GstMobileNo")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("GST_MOBILENO");

                    b.Property<string>("GstRemarks")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("GST_REMARKS");

                    b.Property<string>("GstSerNo")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("GST_SERNO");

                    b.Property<string>("GstState")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("GST_STATE");

                    b.Property<string>("GstTinNo")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("GST_TINNO");

                    b.Property<string>("GstVendCity")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_VENDCITY");

                    b.Property<string>("GstVendCityName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("GST_VENDCITYNAME");

                    b.Property<string>("GstVendPincode")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)")
                        .HasColumnName("GST_VENDPINCODE");

                    b.HasKey("GstTinId");

                    b.HasIndex("GstId");

                    b.ToTable("GST_STATEREGDET", (string)null);
                });

            modelBuilder.Entity("GSTComplianceService.Domain.Entities.GstSupplier", b =>
                {
                    b.Property<long>("SupplierNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("SUPPLIER_NUMBER");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("SupplierNumber"));

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("EMAIL_ADDRESS");

                    b.Property<string>("OperatingUnit")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("OU");

                    b.Property<string>("PanNo")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PAN_NO");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("SUPPLIER_NAME");

                    b.HasKey("SupplierNumber");

                    b.ToTable("GST_SUPPLIER", (string)null);
                });

            modelBuilder.Entity("GSTComplianceService.Domain.Entities.GstHsnDetail", b =>
                {
                    b.HasOne("GSTComplianceService.Domain.Entities.GstMain", "GstMain")
                        .WithMany("HsnDetails")
                        .HasForeignKey("GstHsnGstId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GstMain");
                });

            modelBuilder.Entity("GSTComplianceService.Domain.Entities.GstServiceDetail", b =>
                {
                    b.HasOne("GSTComplianceService.Domain.Entities.GstMain", "GstMain")
                        .WithMany("ServiceDetails")
                        .HasForeignKey("GstSacGstId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GstMain");
                });

            modelBuilder.Entity("GSTComplianceService.Domain.Entities.GstStateRegDetail", b =>
                {
                    b.HasOne("GSTComplianceService.Domain.Entities.GstMain", "GstMain")
                        .WithMany("StateRegDetails")
                        .HasForeignKey("GstId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GstMain");
                });

            modelBuilder.Entity("GSTComplianceService.Domain.Entities.GstMain", b =>
                {
                    b.Navigation("HsnDetails");
                    b.Navigation("ServiceDetails");
                    b.Navigation("StateRegDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
