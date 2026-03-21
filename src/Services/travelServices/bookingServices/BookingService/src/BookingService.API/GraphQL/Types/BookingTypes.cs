using BookingService.Domain.Entities;
using BookingService.Infrastructure.Data;
using HotChocolate;
using HotChocolate.Data;

namespace BookingService.API.GraphQL.Types;

public class BookingRequestType : ObjectType<BookingRequest>
{
    protected override void Configure(IObjectTypeDescriptor<BookingRequest> descriptor)
    {
        descriptor.Description("A travel booking request.");
        descriptor.Field(f => f.BkBokNum).Name("bookingNumber").Description("Unique booking request number");
        descriptor.Field(f => f.BkUsrCod).Name("userCode").Description("Employee user code");
        descriptor.Field(f => f.BkBokTyp).Name("bookingType").Description("S=Stay, T=Travel, L=LocalConveyance");
        descriptor.Field(f => f.BkPerNam).Name("personName");
        descriptor.Field(f => f.BkFroDat).Name("departureDate");
        descriptor.Field(f => f.BkRetDat).Name("returnDate");
        descriptor.Field(f => f.BkFroCit).Name("fromCity");
        descriptor.Field(f => f.BkToCit).Name("toCity");
        descriptor.Field(f => f.BkFroLoc).Name("fromLocation");
        descriptor.Field(f => f.BkToLoc).Name("toLocation");
        descriptor.Field(f => f.BkAppSts).Name("status");
        descriptor.Field(f => f.BkBudAmt).Name("budgetAmount");
        descriptor.Field(f => f.BkCnfNum).Name("confirmationNumber");
        descriptor.Field(f => f.BkCanDat).Name("cancelledOn");
        descriptor.Field(f => f.BkCanRem).Name("cancellationRemarks");
    }
}

public class BookingConfirmationType : ObjectType<BookingConfirmation>
{
    protected override void Configure(IObjectTypeDescriptor<BookingConfirmation> descriptor)
    {
        descriptor.Description("A booking confirmation record.");
        descriptor.Field(f => f.BkCnfNum).Name("confirmationNumber");
        descriptor.Field(f => f.BkBokNum).Name("bookingNumber");
        descriptor.Field(f => f.BkModCod).Name("modeOfTravel");
        descriptor.Field(f => f.BkFroCit).Name("fromCity");
        descriptor.Field(f => f.BkToCit).Name("toCity");
        descriptor.Field(f => f.BkFroDat).Name("departureDate");
        descriptor.Field(f => f.BkToDat).Name("returnDate");
        descriptor.Field(f => f.BkVndCod).Name("vendorCode");
        descriptor.Field(f => f.BkTckNum).Name("ticketNumber");
        descriptor.Field(f => f.BkAdmRmk).Name("adminRemarks");
        descriptor.Field(f => f.BkStsCod).Name("status");
    }
}
