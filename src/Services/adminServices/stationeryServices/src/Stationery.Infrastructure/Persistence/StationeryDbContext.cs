using Microsoft.EntityFrameworkCore;
using Stationery.Domain.Common;
using Stationery.Domain.Entities;

namespace Stationery.Infrastructure.Persistence;

public class StationeryDbContext : DbContext
{
    public StationeryDbContext(DbContextOptions<StationeryDbContext> options) : base(options) { }

    public DbSet<StationaryMaster> StationaryMasters => Set<StationaryMaster>();
    public DbSet<RequestMain> RequestMains => Set<RequestMain>();
    public DbSet<RequestSub> RequestSubs => Set<RequestSub>();
    public DbSet<OrderMain> OrderMains => Set<OrderMain>();
    public DbSet<OrderSub> OrderSubs => Set<OrderSub>();
    public DbSet<DeptBudget> DeptBudgets => Set<DeptBudget>();
    public DbSet<UnitBudget> UnitBudgets => Set<UnitBudget>();
    public DbSet<DeptApprover> DeptApprovers => Set<DeptApprover>();
    public DbSet<UnitApprover> UnitApprovers => Set<UnitApprover>();
    public DbSet<LocationAdmin> LocationAdmins => Set<LocationAdmin>();
    public DbSet<StationeryReorderAlert> ReorderAlerts => Set<StationeryReorderAlert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Prevent EF from mapping DomainEvent (it's a MediatR notification, not a DB entity)
        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.Entity<StationaryMaster>(entity => {
            entity.ToTable("STATIONARY_MASTER");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("SM_STATIONARYID");
            entity.Property(e => e.CatId).HasColumnName("SM_CATID");
            entity.Property(e => e.LocId).HasColumnName("SM_LOC_ID");
            entity.Property(e => e.Description).HasColumnName("SM_DESC").HasMaxLength(200);
            entity.Property(e => e.UomId).HasColumnName("SM_UOMID");
            entity.Property(e => e.Make).HasColumnName("SM_MAKE").HasMaxLength(10);
            entity.Property(e => e.PricePerUnit).HasColumnName("SM_PRICE_PERUNIT");
            entity.Property(e => e.ReorderLevel).HasColumnName("SM_REORDER_LEVEL");
            entity.Property(e => e.UpdatedBy).HasColumnName("SM_UPDATED_BY");
            entity.Property(e => e.UpdatedOn).HasColumnName("SM_UPDATED_ON");
            entity.Property(e => e.VmId).HasColumnName("SM_VMID");
            entity.Property(e => e.Closed).HasColumnName("SM_CLOSED");
            entity.Property(e => e.OpeningStock).HasColumnName("SM_OPENINGSTOCK");
        });

        modelBuilder.Entity<RequestMain>(entity => {
            entity.ToTable("SP_REQUEST_MAIN");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("RM_REQUESTID");
            entity.Property(e => e.RequestedBy).HasColumnName("RM_REQUESTEDBY");
            entity.Property(e => e.RequestedOn).HasColumnName("RM_REQUESTEDON");
            entity.Property(e => e.LocationId).HasColumnName("RM_LOCATIONID");
            entity.Property(e => e.UnitCode).HasColumnName("RM_UNITCODE").IsFixedLength().HasMaxLength(3);
        });

        modelBuilder.Entity<RequestSub>(entity => {
            entity.ToTable("SP_REQUEST_SUB");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("RS_REQUESTSUB_ID");
            entity.Property(e => e.RequestId).HasColumnName("RS_REQUESTID");
            entity.Property(e => e.StationaryId).HasColumnName("RS_STATIONARYID");
            entity.Property(e => e.DeptId).HasColumnName("RS_DEPTID");
            entity.Property(e => e.ExpectedDate).HasColumnName("RS_EXPECTED_DATE");
            entity.Property(e => e.UserSysId).HasColumnName("RS_USER_SYSID");
            entity.Property(e => e.RequestedQty).HasColumnName("RS_REQUESTEDQTY");
            entity.Property(e => e.IndentedQty).HasColumnName("RS_INDENTEDQTY");
            entity.Property(e => e.ApprovedQty).HasColumnName("RS_APPROVEDQTY");
            entity.Property(e => e.ApproverSysId).HasColumnName("RS_APPROVER_SYSID");
            entity.Property(e => e.ApproverRemarks).HasColumnName("RS_APPROVER_RAMARKS").HasMaxLength(255);
            entity.Property(e => e.ReceivedDate).HasColumnName("RS_RECEIVED_DATE");
            entity.Property(e => e.Status).HasColumnName("RS_STATUS").HasMaxLength(1);
            entity.Property(e => e.UpdatedBy).HasColumnName("RS_UPDATED_BY");
            entity.Property(e => e.UpdatedOn).HasColumnName("RS_UPDATED_ON");
            entity.Property(e => e.ApprovedOn).HasColumnName("RS_APPROVED_ON");

            entity.HasOne(d => d.Request)
                .WithMany(p => p.Details)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderMain>(entity => {
            entity.ToTable("SP_ORDER_MAIN");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("OM_ORDERMAIN_ID");
            entity.Property(e => e.LocationId).HasColumnName("OM_LOCATION_ID");
            entity.Property(e => e.VendorId).HasColumnName("OM_VENDORID");
            entity.Property(e => e.DeliveryDate).HasColumnName("OM_DELIVERYDATE");
            entity.Property(e => e.OrderedDate).HasColumnName("OM_ORDEREDDATE");
            entity.Property(e => e.OrderedBy).HasColumnName("OM_ORDEREDBY");
        });

        modelBuilder.Entity<OrderSub>(entity => {
            entity.ToTable("SP_ORDER_SUB");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("OS_ORDERSUB_ID");
            entity.Property(e => e.OrderMainId).HasColumnName("OS_ORDERMAIN_ID");
            entity.Property(e => e.RequestSubId).HasColumnName("OS_REQUESTSUB_ID");
            entity.Property(e => e.OrderedQty).HasColumnName("OS_ORDERED_QTY");
            entity.Property(e => e.ReceivedOn).HasColumnName("OS_RECEIVEDON");
            entity.Property(e => e.ReceivedBy).HasColumnName("OS_RECEIVED_BY");
            entity.Property(e => e.OrderPrice).HasColumnName("OS_ORDERPRICE");
            entity.Property(e => e.ActualPrice).HasColumnName("OS_ACTUALPRICE");
            entity.Property(e => e.ReceivedDate).HasColumnName("OS_RECEIVEDDATE");
            entity.Property(e => e.DeliveryDate).HasColumnName("OS_DELIVERYDATE");
            entity.Property(e => e.ReceiptEntryBy).HasColumnName("OS_RECEIPTENTRYBY");
            entity.Property(e => e.ReceiptEntryOn).HasColumnName("OS_RECEIPTENTRYON");

            entity.HasOne(d => d.OrderMain)
                .WithMany(p => p.Details)
                .HasForeignKey(d => d.OrderMainId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DeptBudget>(entity => {
            entity.ToTable("SP_DEPT_BUDGET");
            entity.HasKey(e => new { e.LocId, e.DeptId, e.FinYearId });
            entity.Property(e => e.LocId).HasColumnName("DB_LOCATION_ID");
            entity.Property(e => e.UnitCode).HasColumnName("DB_UNIT_CODE").IsFixedLength().HasMaxLength(3);
            entity.Property(e => e.DeptId).HasColumnName("DB_DEPT_ID");
            entity.Property(e => e.FinYearId).HasColumnName("DB_FINYEAR_ID");
            entity.Property(e => e.BudgetAmount).HasColumnName("DB_BUDGETAMOUNT");
            entity.Property(e => e.UpdatedBy).HasColumnName("DB_UPDATED_BY");
            entity.Property(e => e.UpdatedOn).HasColumnName("DB_UPDATED_ON");
        });

        modelBuilder.Entity<UnitBudget>(entity => {
            entity.ToTable("SP_UNIT_BUDGET");
            entity.HasKey(e => new { e.LocId, e.UnitCode, e.FinYearId });
            entity.Property(e => e.LocId).HasColumnName("UB_LOCATION_ID");
            entity.Property(e => e.UnitCode).HasColumnName("UB_UNIT_CODE").IsFixedLength().HasMaxLength(3);
            entity.Property(e => e.FinYearId).HasColumnName("UB_FINYEAR_ID");
            entity.Property(e => e.BudgetAmount).HasColumnName("UB_BUDGETAMOUNT");
            entity.Property(e => e.UpdatedBy).HasColumnName("UB_UPDATED_BY");
            entity.Property(e => e.UpdatedOn).HasColumnName("UB_UPDATED_ON");
        });

        modelBuilder.Entity<DeptApprover>(entity => {
            entity.ToTable("SP_DEPT_APPROVER");
            entity.HasKey(e => new { e.LocationId, e.DeptId, e.EmpSysId, e.Type });
            entity.Property(e => e.LocationId).HasColumnName("DA_LOCATION_ID");
            entity.Property(e => e.UnitCode).HasColumnName("DA_UNIT_CODE").IsFixedLength().HasMaxLength(3);
            entity.Property(e => e.DeptId).HasColumnName("DA_DEPT_ID");
            entity.Property(e => e.EmpSysId).HasColumnName("DA_EMP_SYSID");
            entity.Property(e => e.Type).HasColumnName("DA_TYPE").IsFixedLength().HasMaxLength(1);
            entity.Property(e => e.EffectiveDate).HasColumnName("DA_EFFECTIVE_DATE");
            entity.Property(e => e.ClosureDate).HasColumnName("DA_CLOSURE_DATE");
            entity.Property(e => e.UpdatedBy).HasColumnName("DA_UPDATED_BY");
            entity.Property(e => e.UpdatedOn).HasColumnName("DA_UPDATED_ON");
        });

        modelBuilder.Entity<UnitApprover>(entity => {
            entity.ToTable("SP_UNIT_APPROVER");
            entity.HasKey(e => new { e.LocationId, e.UnitCode, e.EmpSysId, e.Type });
            entity.Property(e => e.LocationId).HasColumnName("UA_LOCATION_ID");
            entity.Property(e => e.UnitCode).HasColumnName("UA_UNIT_CODE").IsFixedLength().HasMaxLength(3);
            entity.Property(e => e.EmpSysId).HasColumnName("UA_EMP_SYSID");
            entity.Property(e => e.Type).HasColumnName("UA_TYPE").IsFixedLength().HasMaxLength(1);
            entity.Property(e => e.EffectiveDate).HasColumnName("UA_EFFECTIVE_DATE");
            entity.Property(e => e.ClosureDate).HasColumnName("UA_CLOSURE_DATE");
            entity.Property(e => e.UpdatedBy).HasColumnName("UA_UPDATED_BY");
            entity.Property(e => e.UpdatedOn).HasColumnName("UA_UPDATED_ON");
        });

        modelBuilder.Entity<LocationAdmin>(entity => {
            entity.ToTable("SP_LOCATION_ADMIN");
            entity.HasKey(e => new { e.LocationId, e.EmpSysId });
            entity.Property(e => e.LocationId).HasColumnName("LA_LOCATION_ID");
            entity.Property(e => e.EmpSysId).HasColumnName("LA_EMP_SYSID");
            entity.Property(e => e.EffectiveDate).HasColumnName("LA_EFFECTIVE_DATE");
            entity.Property(e => e.ClosureDate).HasColumnName("LA_CLOSURE_DATE");
            entity.Property(e => e.UpdatedBy).HasColumnName("LA_UPDATED_BY");
            entity.Property(e => e.UpdatedOn).HasColumnName("LA_UPDATED_ON");
        });

        modelBuilder.Entity<StationeryReorderAlert>(entity => {
            entity.ToTable("STATIONERY_REORDER_ALERT");
            entity.HasKey(e => e.AlertId);
            entity.Property(e => e.AlertId).HasColumnName("AlertID").ValueGeneratedOnAdd();
            entity.Property(e => e.StationaryId).HasColumnName("StationaryID");
            entity.Property(e => e.AlertDate).HasColumnName("AlertDate");
            entity.Property(e => e.CurrentStock).HasColumnName("CurrentStock");
            entity.Property(e => e.ReorderLevel).HasColumnName("ReorderLevel");
            entity.Property(e => e.Resolved).HasColumnName("Resolved").IsFixedLength().HasMaxLength(1);
        });
    }
}
