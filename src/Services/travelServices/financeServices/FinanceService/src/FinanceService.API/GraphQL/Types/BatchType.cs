using FinanceService.Domain.Entities;
using HotChocolate.Types;

namespace FinanceService.API.GraphQL.Types;

public class BatchType : ObjectType<TravelBatchMain>
{
    protected override void Configure(IObjectTypeDescriptor<TravelBatchMain> descriptor)
    {
        descriptor.Description("Represents a Travel Batch.");
        descriptor.BindFieldsExplicitly();
        descriptor.Field(f => f.UnitCode);
        descriptor.Field(f => f.BatchNumber);
        descriptor.Field(f => f.BatchDate);
        descriptor.Field(f => f.InvoiceNumber);
        descriptor.Field(f => f.InvoiceDate);
        descriptor.Field(f => f.BatchStatus);
        descriptor.Field(f => f.AdminRemarks);
        descriptor.Field(f => f.FinanceRemarks);
        descriptor.Field(f => f.AgencyCode);
        descriptor.Field(f => f.TotalApprovedAmount);
        descriptor.Field(f => f.Total);
        descriptor.Field(f => f.CgstAmount);
        descriptor.Field(f => f.SgstAmount);
        descriptor.Field(f => f.IgstAmount);
        descriptor.Field(f => f.BatchLines);
    }
}

public class BatchSubType : ObjectType<TravelBatchSub>
{
    protected override void Configure(IObjectTypeDescriptor<TravelBatchSub> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(f => f.UnitCode);
        descriptor.Field(f => f.BatchNumber);
        descriptor.Field(f => f.SerialNumber);
        descriptor.Field(f => f.BookingNumber);
        descriptor.Field(f => f.TicketCost);
        descriptor.Field(f => f.ApprovedAmount);
        descriptor.Field(f => f.Status);
    }
}
