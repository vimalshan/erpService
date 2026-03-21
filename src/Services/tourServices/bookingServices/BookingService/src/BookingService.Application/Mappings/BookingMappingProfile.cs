using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Domain.Entities;

namespace BookingService.Application.Mappings;

public class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<BookRequestMain, BookRequestMainDto>().ReverseMap();
        CreateMap<BookRequestTicket, BookRequestTicketDto>().ReverseMap();
        CreateMap<BookRequestStay, BookRequestStayDto>().ReverseMap();
        CreateMap<BookRequestCab, BookRequestCabDto>().ReverseMap();
        CreateMap<BookRequestCostCentre, BookRequestCostCentreDto>().ReverseMap();
        CreateMap<BookRequestOther, BookRequestOtherDto>().ReverseMap();
        CreateMap<BookRequestConfirmation, BookConfirmationDto>().ReverseMap();
    }
}
