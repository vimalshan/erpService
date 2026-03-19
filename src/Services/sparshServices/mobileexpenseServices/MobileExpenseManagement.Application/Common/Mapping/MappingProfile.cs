namespace MobileExpenseManagement.Application.Common.Mapping;

using AutoMapper;
using MobileExpenseManagement.Application.DTOs;
using MobileExpenseManagement.Domain.Entities;

/// <summary>
/// AutoMapper profile for entity to DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Expense mappings
        CreateMap<Expense, ExpenseDto>()
            .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files))
            .ReverseMap();

        CreateMap<ExpenseFile, ExpenseFileDto>().ReverseMap();

        // Create mappings
        CreateMap<CreateExpenseDto, Expense>().ConstructUsing((src, ctx) =>
            Expense.Create(src.TripId, src.CategoryId, src.ExpenseDate, src.Comment, 
                src.Amount, src.CurrencyId, 0));

        CreateMap<UpdateExpenseDto, Expense>();
    }
}
