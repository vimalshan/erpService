using SettlementService.Application.DTOs;
using SettlementService.Domain.Aggregates;

namespace SettlementService.API.GraphQL;

public class SettlementType : ObjectType<SettlementDto>
{
    protected override void Configure(IObjectTypeDescriptor<SettlementDto> descriptor)
    {
        descriptor.Name("Settlement");

        descriptor.Field(f => f.SettlementNumber).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.TrustCode).Type<StringType>();
        descriptor.Field(f => f.MemberNo).Type<LongType>();
        descriptor.Field(f => f.SettlementType).Type<StringType>();
        descriptor.Field(f => f.SettlementDate).Type<DateTimeType>();
        descriptor.Field(f => f.SettlementAmount).Type<DecimalType>();
        descriptor.Field(f => f.Status).Type<StringType>();
        descriptor.Field(f => f.Reason).Type<StringType>();
        descriptor.Field(f => f.Deductions).Type<ListType<NonNullType<ObjectType<DeductionDto>>>>();
        descriptor.Field(f => f.Approvals).Type<ListType<NonNullType<ObjectType<ApprovalDto>>>>();
        descriptor.Field(f => f.Payments).Type<ListType<NonNullType<ObjectType<PaymentDto>>>>();
    }
}
