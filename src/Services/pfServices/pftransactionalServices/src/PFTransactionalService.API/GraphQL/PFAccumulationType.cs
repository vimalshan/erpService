using PFTransactionalService.Application.DTOs;

namespace PFTransactionalService.API.GraphQL;

public class PFAccumulationType : ObjectType<PFAccumulationDto>
{
    protected override void Configure(IObjectTypeDescriptor<PFAccumulationDto> descriptor)
    {
        descriptor.Name("PFAccumulation");

        descriptor.Field(f => f.PfAccId).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.EmpSysId).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.MemberNo).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.TrustCode).Type<StringType>();
        descriptor.Field(f => f.PfAccBal).Type<NonNullType<DecimalType>>();
        descriptor.Field(f => f.PfEmpContTotal).Type<DecimalType>();
        descriptor.Field(f => f.PfErContTotal).Type<DecimalType>();
        descriptor.Field(f => f.PfVolContTotal).Type<DecimalType>();
        descriptor.Field(f => f.PfAccStatus).Type<StringType>();
        descriptor.Field(f => f.Contributions).Type<ListType<NonNullType<ObjectType<ContributionTxnDto>>>>();
        descriptor.Field(f => f.Certificates).Type<ListType<NonNullType<ObjectType<WithdrawalCertificateDto>>>>();
    }
}
