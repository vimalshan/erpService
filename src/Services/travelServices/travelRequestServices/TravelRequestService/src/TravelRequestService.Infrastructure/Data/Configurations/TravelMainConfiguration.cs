using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelRequestService.Domain.Entities;
using TravelRequestService.Domain.Enums;

namespace TravelRequestService.Infrastructure.Data.Configurations;

public class TravelMainConfiguration : IEntityTypeConfiguration<TravelMain>
{
    public void Configure(EntityTypeBuilder<TravelMain> builder)
    {
        builder.ToTable("TRAVEL_MAIN");

        builder.HasKey(e => new { e.PlanNumber, e.CompanyCode });

        builder.Property(e => e.CompanyCode).HasColumnName("TR_COM_COD").HasColumnType("char(3)").IsRequired();
        builder.Property(e => e.PlanNumber).HasColumnName("TR_PLN_NUM").HasColumnType("bigint").IsRequired();
        builder.Property(e => e.UserCode).HasColumnName("TR_USR_COD").HasMaxLength(20);
        builder.Property(e => e.UserNumber).HasColumnName("TR_USR_NUM");
        builder.Property(e => e.AppliedDate).HasColumnName("TR_APP_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedDate).HasColumnName("TR_MOD_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedBy).HasColumnName("TR_MOD_USR").HasMaxLength(20);
        builder.Property(e => e.NatureCode).HasColumnName("TR_NAT_COD");
        builder.Property(e => e.ObjectiveDescription).HasColumnName("TR_OBJ_DES").HasMaxLength(200);
        builder.Property(e => e.Remarks).HasColumnName("TR_REM_MRK").HasMaxLength(200);
        builder.Property(e => e.TripOutcome).HasColumnName("TR_OUT_COM").HasMaxLength(200);

        builder.Property(e => e.IsBudgeted).HasColumnName("TR_BUD_FLG")
            .HasConversion(v => v ? "Y" : "N", v => v == "Y")
            .HasColumnType("char(1)");

        builder.Property(e => e.Status).HasColumnName("TR_PLS_FLG")
            .HasConversion(
                v => (char)v,
                v => (TravelRequestStatus)v)
            .HasColumnType("char(1)");

        builder.Property(e => e.SettlementStatus).HasColumnName("TR_SET_STS")
            .HasConversion(
                v => v.HasValue ? (char?)v.Value : null,
                v => v.HasValue ? (SettlementStatus?)v.Value : null)
            .HasColumnType("char(1)");

        builder.Property(e => e.TripFlag).HasColumnName("TR_TRP_FLG")
            .HasConversion(v => v.HasValue ? (v.Value ? "Y" : "N") : null, v => v != null ? v == "Y" : (bool?)null)
            .HasColumnType("char(1)");

        builder.Property(e => e.BudgetAmount).HasColumnName("TR_BUD_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.ActualAmount).HasColumnName("TR_ACT_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.AdvanceAmount).HasColumnName("TR_ADV_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PaidAmount).HasColumnName("TR_PAD_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.AdjustedAmount).HasColumnName("TR_ADJ_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.RequestId).HasColumnName("TR_REQ_ID").HasColumnType("decimal(20,0)");

        builder.Property(e => e.TravelType).HasColumnName("TR_TVL_TYP")
            .HasConversion(
                v => v == TravelType.Domestic ? "DOM" : "INT",
                v => v == "DOM" ? TravelType.Domestic : TravelType.International)
            .HasColumnType("char(3)");

        builder.Property(e => e.CurrencyPreference).HasColumnName("TR_CUR_PRF")
            .HasConversion(v => v ? "Y" : "N", v => v == "Y")
            .HasColumnType("char(1)");

        builder.Property(e => e.AdditionalAmount).HasColumnName("TR_ADD_AMT").HasColumnType("decimal(19,0)");

        builder.Property(e => e.SpecialSanction).HasColumnName("TR_SPL_SNC")
            .HasConversion(v => v.HasValue ? (v.Value ? "Y" : "N") : null, v => v != null ? v == "Y" : (bool?)null)
            .HasColumnType("char(1)");

        builder.Property(e => e.FinancialUnit).HasColumnName("TR_FIN_UNT");
        builder.Property(e => e.CcrRemarks).HasColumnName("TR_CCR_RMK").HasMaxLength(200);

        builder.Property(e => e.BypassApproval).HasColumnName("TR_BYPASS_APP")
            .HasConversion(v => v.HasValue ? (v.Value ? "Y" : "N") : null, v => v != null ? v == "Y" : (bool?)null)
            .HasColumnType("char(1)");

        builder.Property(e => e.AccountTender).HasColumnName("TR_ACC_TEN")
            .HasConversion(v => v.HasValue ? (v.Value ? "Y" : "N") : null, v => v != null ? v == "Y" : (bool?)null)
            .HasColumnType("char(1)");

        builder.Property(e => e.BypassRemarks).HasColumnName("TR_BYPASS_REM").HasMaxLength(200);

        // Ignore domain event fields
        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.Version);

        // Navigation properties
        builder.HasMany(e => e.Agendas).WithOne().HasForeignKey(a => a.RequestNumber).HasPrincipalKey(t => t.PlanNumber);
        builder.HasMany(e => e.Advances).WithOne().HasForeignKey(a => a.RequestNumber).HasPrincipalKey(t => t.PlanNumber);
        builder.HasMany(e => e.ApprovalRemarks).WithOne().HasForeignKey(a => a.RequestNumber).HasPrincipalKey(t => t.PlanNumber);
        builder.HasMany(e => e.SubDetails).WithOne().HasForeignKey(s => s.RequestNumber).HasPrincipalKey(t => t.PlanNumber);
    }
}
