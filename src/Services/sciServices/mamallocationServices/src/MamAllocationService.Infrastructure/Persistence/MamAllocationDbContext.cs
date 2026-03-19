using MamAllocationService.Domain.Common;
using MamAllocationService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MamAllocationService.Infrastructure.Persistence;

public class MamAllocationDbContext(DbContextOptions<MamAllocationDbContext> options, IMediator mediator)
    : DbContext(options)
{
    public DbSet<AllocationDetail> AllocationDetails => Set<AllocationDetail>();
    public DbSet<AllocationProdDetail> AllocationProdDetails => Set<AllocationProdDetail>();
    public DbSet<AllocationFg> AllocationFgs => Set<AllocationFg>();
    public DbSet<ArrivalDetail> ArrivalDetails => Set<ArrivalDetail>();
    public DbSet<ConsumptionDetail> ConsumptionDetails => Set<ConsumptionDetail>();
    public DbSet<DispatchDetail> DispatchDetails => Set<DispatchDetail>();
    public DbSet<FgAllocation> FgAllocations => Set<FgAllocation>();
    public DbSet<ProductAllocation> ProductAllocations => Set<ProductAllocation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllocationDetail>(eb =>
        {
            eb.ToTable("MAM_ALLOCATION_DET");
            eb.HasKey(e => new { e.AllDate, e.AllRm });
            eb.Property(e => e.AllDate).HasColumnName("ALL_DATE").HasColumnType("datetime2(3)");
            eb.Property(e => e.AllRm).HasColumnName("ALL_RM");
            eb.Property(e => e.AllEntOpen).HasColumnName("ALL_ENTOPEN").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllFormIvdDf).HasColumnName("ALL_FORMIVDDF").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllFormIvIdF).HasColumnName("ALL_FORMIVIDF").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllFormIvIdP).HasColumnName("ALL_FORMIVIDP").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllFormIvDdP).HasColumnName("ALL_FORMIVDDP").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllFormIvIdFWo).HasColumnName("ALL_FORMIVIDFWO").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllFormIvDdFWo).HasColumnName("ALL_FORMIVDDFWO").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllClosedDf).HasColumnName("ALL_CLOSEDDF").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllCloseIdF).HasColumnName("ALL_CLOSEIDF").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllCloseIdP).HasColumnName("ALL_CLOSEIDP").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllCloseDdP).HasColumnName("ALL_CLOSEDDP").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllEntDebit).HasColumnName("ALL_ENTDEBIT").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllProdEntDebit).HasColumnName("ALL_PRODENTDEBIT").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllDispEntCredit).HasColumnName("ALL_DISPENTCREDIT").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllNetEnt).HasColumnName("ALL_NETENT").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllAddDdf).HasColumnName("ALL_ADDDDF").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllAddIdF).HasColumnName("ALL_ADDIDF").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllAddIdP).HasColumnName("ALL_ADDIDP").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllAddDdP).HasColumnName("ALL_ADDDDP").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllProd).HasColumnName("ALL_PROD").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllCons).HasColumnName("ALL_CONS").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllRg1Ddf).HasColumnName("ALL_RG1DDF").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllRg1Ddp).HasColumnName("ALL_RG1DDP").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllCloseRg1Ddf).HasColumnName("ALL_CLOSERG1DDF").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllCloseRg1Ddp).HasColumnName("ALL_CLOSERG1DDP").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllSaleFormIvIdP).HasColumnName("ALL_SALEFORMIVIDP").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllSaleFormIvDdP).HasColumnName("ALL_SALEFORMIVDDP").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllSaleRg1Ddp).HasColumnName("ALL_SALERG1DDP").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllSale).HasColumnName("ALL_SALE").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllAddRgDdf).HasColumnName("ALL_ADDRGDDF").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllAddRgDdp).HasColumnName("ALL_ADDRGDDP").HasColumnType("decimal(19,0)");
            eb.Ignore(e => e.DomainEvents);
        });

        modelBuilder.Entity<AllocationProdDetail>(eb =>
        {
            eb.ToTable("MAM_ALLOCATION_PRODDET");
            eb.HasNoKey();
            eb.Property(e => e.AllDate).HasColumnName("ALL_DATE").HasColumnType("datetime2(3)");
            eb.Property(e => e.AllSrl).HasColumnName("ALL_SRL");
            eb.Property(e => e.AllFg).HasColumnName("ALL_FG");
            eb.Property(e => e.DdfQty).HasColumnName("DDF_QTY").HasColumnType("decimal(19,0)");
            eb.Property(e => e.DdpQty).HasColumnName("DDP_QTY").HasColumnType("decimal(19,0)");
            eb.Property(e => e.PrdQty).HasColumnName("PRD_QTY").HasColumnType("decimal(19,0)");
            eb.Property(e => e.AllRm).HasColumnName("ALL_RM").HasColumnType("decimal(19,0)");
            eb.Ignore(e => e.DomainEvents);
        });

        modelBuilder.Entity<AllocationFg>(eb =>
        {
            eb.ToTable("MAM_ALLOCATIONFG");
            eb.HasNoKey();
            eb.Property(e => e.AllDate).HasColumnName("ALL_DATE").HasColumnType("datetime2(3)");
            eb.Property(e => e.FgCode).HasColumnName("FG_CODE");
            eb.Property(e => e.DomDispatch).HasColumnName("DOM_DISPATCH");
            eb.Property(e => e.ExpDispatch).HasColumnName("EXP_DISPATCH").HasColumnType("decimal(19,0)");
            eb.Property(e => e.DutyFree).HasColumnName("DUTY_FREE").HasColumnType("decimal(19,0)");
            eb.Property(e => e.DutyPaid).HasColumnName("DUTY_PAID").HasColumnType("decimal(19,0)");
            eb.Ignore(e => e.DomainEvents);
        });

        modelBuilder.Entity<ArrivalDetail>(eb =>
        {
            eb.ToTable("MAM_ARRIVAL_DET");
            eb.HasNoKey();
            eb.Property(e => e.ArrivalNo).HasColumnName("ARRIVAL_NO");
            eb.Property(e => e.ArrivalDate).HasColumnName("ARRIVAL_DATE").HasColumnType("datetime2(3)");
            eb.Property(e => e.ArrivalQty).HasColumnName("ARRIVAL_QTY").HasColumnType("decimal(19,0)");
            eb.Property(e => e.ArrivalItem).HasColumnName("ARRIVAL_ITEM");
            eb.Property(e => e.ArrivalReceiptNo).HasColumnName("ARRIVAL_RECEIPTNO").HasColumnType("decimal(38,0)");
            eb.Ignore(e => e.DomainEvents);
        });

        modelBuilder.Entity<ConsumptionDetail>(eb =>
        {
            eb.ToTable("MAM_CONSUMPTION_DET");
            eb.HasNoKey();
            eb.Property(e => e.ConsumptionNo).HasColumnName("CONSUMPTION_NO");
            eb.Property(e => e.ConsumptionDate).HasColumnName("CONSUMPTION_DATE").HasColumnType("datetime2(3)");
            eb.Property(e => e.ConsumptionRm).HasColumnName("CONSUMPTION_RM");
            eb.Property(e => e.ConsumptionQty).HasColumnName("CONSUMPTION_QTY").HasColumnType("decimal(19,0)");
            eb.Ignore(e => e.DomainEvents);
        });

        modelBuilder.Entity<DispatchDetail>(eb =>
        {
            eb.ToTable("MAM_DISPATCH_DET");
            eb.HasNoKey();
            eb.Property(e => e.DispatchNo).HasColumnName("DISPATCH_NO").HasColumnType("decimal(38,0)");
            eb.Property(e => e.DispatchDate).HasColumnName("DISPATCH_DATE").HasColumnType("datetime2(3)");
            eb.Property(e => e.DispatchFg).HasColumnName("DISPATCH_FG");
            eb.Property(e => e.DispatchQty).HasColumnName("DISPATCH_QTY").HasColumnType("decimal(19,0)");
            eb.Property(e => e.DispatchType).HasColumnName("DISPATCH_TYPE").HasColumnType("char(1)");
            eb.Property(e => e.DispatchAreDate).HasColumnName("DISPATCH_AREDATE").HasColumnType("datetime2(3)");
            eb.Property(e => e.DispatchInvoiceNo).HasColumnName("DISPATCH_INVOICENO").HasMaxLength(20);
            eb.Property(e => e.DispatchAdvNo).HasColumnName("DISPATCH_ADVNO");
            eb.Ignore(e => e.DomainEvents);
        });

        modelBuilder.Entity<FgAllocation>(eb =>
        {
            eb.ToTable("MAM_FG_ALLOCATION");
            eb.HasNoKey();
            eb.Property(e => e.Sno).HasColumnName("SNO");
            eb.Property(e => e.FgCode).HasColumnName("FG_CODE");
            eb.Property(e => e.Flag).HasColumnName("FLAG").HasColumnType("char(1)");
            eb.Ignore(e => e.DomainEvents);
        });

        modelBuilder.Entity<ProductAllocation>(eb =>
        {
            eb.ToTable("MAM_PRODUCT_ALLOCATION");
            eb.HasNoKey();
            eb.Property(e => e.Sno).HasColumnName("SNO");
            eb.Property(e => e.RmCode).HasColumnName("RM_CODE");
            eb.Ignore(e => e.DomainEvents);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.DomainEvents).ToList();
        domainEntities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent, ct);

        return result;
    }
}
