using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.API.GraphQL.Types;

public class LovTypeMasterType : ObjectType<LovTypeMasterDto>
{
    protected override void Configure(IObjectTypeDescriptor<LovTypeMasterDto> descriptor)
    {
        descriptor.Name("LovTypeMaster");
        descriptor.Field(f => f.LovTypeCode).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.LovTypeName).Type<StringType>();
    }
}
