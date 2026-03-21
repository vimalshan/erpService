using EmployeeService.Application.DTOs;

namespace EmployeeService.API.GraphQL.Types;

public class EmployeeType : ObjectType<EmployeeDto>
{
    protected override void Configure(IObjectTypeDescriptor<EmployeeDto> descriptor)
    {
        descriptor.Name("Employee");

        descriptor.Field(e => e.EmployeeId).Type<NonNullType<IntType>>();
        descriptor.Field(e => e.FirstName).Type<NonNullType<StringType>>();
        descriptor.Field(e => e.LastName).Type<NonNullType<StringType>>();
        descriptor.Field(e => e.EmployeeCode).Type<NonNullType<StringType>>();
        descriptor.Field(e => e.HireDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(e => e.JobTitle).Type<StringType>();
        descriptor.Field(e => e.Department).Type<StringType>();
        descriptor.Field(e => e.Phone).Type<StringType>();
        descriptor.Field(e => e.Email).Type<StringType>();
        descriptor.Field(e => e.IsActive).Type<NonNullType<BooleanType>>();
    }
}
