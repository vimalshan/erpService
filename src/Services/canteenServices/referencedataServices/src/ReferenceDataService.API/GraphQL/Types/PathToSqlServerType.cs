using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.API.GraphQL.Types;

public class PathToSqlServerType : ObjectType<PathToSqlServerDto>
{
    protected override void Configure(IObjectTypeDescriptor<PathToSqlServerDto> descriptor)
    {
        descriptor.Name("PathToSqlServer");
        descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.CompanyCode).Type<StringType>();
        descriptor.Field(f => f.ServerName).Type<StringType>();
        descriptor.Field(f => f.DatabaseName).Type<StringType>();
        descriptor.Field(f => f.UserId).Type<StringType>();
    }
}
