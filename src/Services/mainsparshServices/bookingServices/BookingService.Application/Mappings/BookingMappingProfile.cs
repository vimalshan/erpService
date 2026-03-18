using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Domain.Entities;

namespace BookingService.Application.Mappings;

public class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<BookMain, BookingDto>()
            .ConstructUsing(src => new BookingDto(
                src.Id, src.BookingAppNo, src.BookingTitle, src.LocationCode,
                src.BookingDate, src.Status.Value, src.CreatedBy, src.CreatedOn,
                src.UpdatedBy, src.UpdatedOn));

        CreateMap<BookMain, BookingDetailDto>()
            .ConstructUsing((src, ctx) => new BookingDetailDto(
                src.Id, src.BookingAppNo, src.BookingTitle, src.LocationCode,
                src.BookingDate, src.Status.Value, src.CreatedBy, src.CreatedOn,
                src.UpdatedBy, src.UpdatedOn,
                ctx.Mapper.Map<IEnumerable<BookRecordDto>>(src.Records),
                ctx.Mapper.Map<IEnumerable<AttendeeDto>>(src.Attendees)));

        CreateMap<BookRecord, BookRecordDto>()
            .ConstructUsing(src => new BookRecordDto(
                src.Id, src.BookingId, src.LocationCode, src.RecDetails,
                src.RecStatus.Value, src.CreatedBy, src.CreatedOn));

        CreateMap<BookAttendee, AttendeeDto>()
            .ConstructUsing(src => new AttendeeDto(
                src.Id, src.BookingId, src.AttendeeSysId, src.AttendeeSerial,
                src.AttendanceStatus.Value, src.CreatedBy, src.CreatedOn));
    }
}
