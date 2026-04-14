using AutoMapper;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Entities;

namespace WMTransactional.Application.Mappings;

public class TransactionalMappingProfile : Profile
{
    public TransactionalMappingProfile()
    {
        CreateMap<PurchaseOrder, PurchaseOrderDto>();
        CreateMap<PurchaseOrderLine, PurchaseOrderLineDto>();
        CreateMap<Receiving, ReceivingDto>();
        CreateMap<ReceivingLine, ReceivingLineDto>();
        CreateMap<SalesOrder, SalesOrderDto>();
        CreateMap<SalesOrderLine, SalesOrderLineDto>();
        CreateMap<Shipment, ShipmentDto>();
        CreateMap<ShipmentLine, ShipmentLineDto>();
    }
}
