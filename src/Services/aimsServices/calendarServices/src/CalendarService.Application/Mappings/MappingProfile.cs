using AutoMapper;
using CalendarService.Application.DTOs;
using CalendarService.Domain.Entities;
using CalendarService.Domain.ValueObjects;

namespace CalendarService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CalendarMaster, CalendarDto>()
            .ForCtorParam("Id", o => o.MapFrom(s => s.CalendarId))
            .ForCtorParam("Name", o => o.MapFrom(s => s.CalendarName))
            .ForCtorParam("UnitId", o => o.MapFrom(s => s.CalendarUnitId))
            .ForCtorParam("EffDate", o => o.MapFrom(s => s.CalendarEffDate))
            .ForCtorParam("ClsDate", o => o.MapFrom(s => s.CalendarClsDate))
            .ForCtorParam("Status", o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<HolidayMaster, HolidayDto>()
            .ForCtorParam("Id", o => o.MapFrom(s => s.HolidayId))
            .ForCtorParam("Date", o => o.MapFrom(s => s.HolidayDate))
            .ForCtorParam("Description", o => o.MapFrom(s => s.HolidayDescription))
            .ForCtorParam("Type", o => o.MapFrom(s => s.HolidayType.ToString()))
            .ForCtorParam("UnitId", o => o.MapFrom(s => s.HolidayUnit));

        CreateMap<ShiftMaster, ShiftDto>()
            .ForCtorParam("Id", o => o.MapFrom(s => s.ShiftId))
            .ForCtorParam("Code", o => o.MapFrom(s => s.ShiftCode))
            .ForCtorParam("Name", o => o.MapFrom(s => s.ShiftName))
            .ForCtorParam("InTime", o => o.MapFrom(s => s.ShiftInTime.ToString("HH:mm")))
            .ForCtorParam("OutTime", o => o.MapFrom(s => s.ShiftOutTime.ToString("HH:mm")))
            .ForCtorParam("Duration", o => o.MapFrom(s => s.ShiftDuration));

        CreateMap<PatternMaster, PatternDto>()
            .ForCtorParam("Id", o => o.MapFrom(s => s.PatternId))
            .ForCtorParam("Name", o => o.MapFrom(s => s.PatternName))
            .ForCtorParam("Description", o => o.MapFrom(s => s.PatternDescription))
            .ForCtorParam("CycleId", o => o.MapFrom(s => s.PatternCycleId));

        CreateMap<PatternDetail, PatternDetailDto>()
            .ForCtorParam("Id", o => o.MapFrom(s => s.PatDetId))
            .ForCtorParam("PatternId", o => o.MapFrom(s => s.PatDetPatternId))
            .ForCtorParam("DayNo", o => o.MapFrom(s => s.PatDetDayNo))
            .ForCtorParam("ShiftId", o => o.MapFrom(s => s.PatDetShiftId))
            .ForCtorParam("ShiftName", o => o.MapFrom(s => s.Shift != null ? s.Shift.ShiftName : null));
    }
}
