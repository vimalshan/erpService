using CardManagement.Application.Common.DTOs;

namespace CardManagement.API.GraphQL.Types;

public class GuestCardType : ObjectType<GuestCardDto>
{
    protected override void Configure(IObjectTypeDescriptor<GuestCardDto> descriptor)
    {
        descriptor.Description("A guest card record.");
        descriptor.Field(x => x.CanteenUnit).Description("The canteen unit (primary key).");
        descriptor.Field(x => x.CardNumber).Description("Unique card number.");
        descriptor.Field(x => x.CardName).Description("Name on the card.");
        descriptor.Field(x => x.IsActive).Description("Whether the card is currently active.");
    }
}

public class CanteenCardMapType : ObjectType<CanteenCardMapDto>
{
    protected override void Configure(IObjectTypeDescriptor<CanteenCardMapDto> descriptor)
    {
        descriptor.Description("Maps a card to a canteen unit.");
    }
}

public class CardSettlementType : ObjectType<CardSettlementDto>
{
    protected override void Configure(IObjectTypeDescriptor<CardSettlementDto> descriptor)
    {
        descriptor.Description("Card settlement record.");
    }
}
