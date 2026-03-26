using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.API.GraphQL.Types;

public class LovMasterType : ObjectType<LovMasterDto>
{
    protected override void Configure(IObjectTypeDescriptor<LovMasterDto> descriptor)
    {
        descriptor.Name("LovMaster");
        descriptor.Field(f => f.LovId).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.LovType).Type<StringType>();
        descriptor.Field(f => f.LovName).Type<StringType>();
    }
}
