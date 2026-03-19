using MamAllocationService.Domain.Common;
using MamAllocationService.Domain.Events;

namespace MamAllocationService.Domain.Entities;

public class AllocationDetail : BaseEntity, IAggregateRoot
{
    public DateTime AllDate { get; set; }
    public int AllRm { get; set; }
    public decimal? AllEntOpen { get; set; }
    public decimal? AllFormIvdDf { get; set; }
    public decimal? AllFormIvIdF { get; set; }
    public decimal? AllFormIvIdP { get; set; }
    public decimal? AllFormIvDdP { get; set; }
    public decimal? AllFormIvIdFWo { get; set; }
    public decimal? AllFormIvDdFWo { get; set; }
    public decimal? AllClosedDf { get; set; }
    public decimal? AllCloseIdF { get; set; }
    public decimal? AllCloseIdP { get; set; }
    public decimal? AllCloseDdP { get; set; }
    public decimal? AllEntDebit { get; set; }
    public decimal? AllProdEntDebit { get; set; }
    public decimal? AllDispEntCredit { get; set; }
    public decimal? AllNetEnt { get; set; }
    public decimal? AllAddDdf { get; set; }
    public decimal? AllAddIdF { get; set; }
    public decimal? AllAddIdP { get; set; }
    public decimal? AllAddDdP { get; set; }
    public decimal? AllProd { get; set; }
    public decimal? AllCons { get; set; }
    public decimal? AllRg1Ddf { get; set; }
    public decimal? AllRg1Ddp { get; set; }
    public decimal? AllCloseRg1Ddf { get; set; }
    public decimal? AllCloseRg1Ddp { get; set; }
    public decimal? AllSaleFormIvIdP { get; set; }
    public decimal? AllSaleFormIvDdP { get; set; }
    public decimal? AllSaleRg1Ddp { get; set; }
    public decimal? AllSale { get; set; }
    public decimal? AllAddRgDdf { get; set; }
    public decimal? AllAddRgDdp { get; set; }

    public void UpdateProduction(decimal production)
    {
        AllProd = production;
        AddDomainEvent(new AllocationUpdatedEvent(AllDate, AllRm, "Production", production));
    }

    public void UpdateConsumption(decimal consumption)
    {
        AllCons = consumption;
        AddDomainEvent(new AllocationUpdatedEvent(AllDate, AllRm, "Consumption", consumption));
    }

    public void UpdateSale(decimal sale)
    {
        AllSale = sale;
        AddDomainEvent(new AllocationUpdatedEvent(AllDate, AllRm, "Sale", sale));
    }
}
