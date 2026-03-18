using LovService.Application.DTOs;
using LovService.Domain.Entities;
using LovService.Domain.Interfaces;
using LovService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LovService.API.GraphQL;

public class LovQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<LovTypeMast> GetLovTypes([Service] LovDbContext db)
        => db.LovTypeMasts.AsNoTracking();

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<LovMaster> GetLovMasters([Service] LovDbContext db)
        => db.LovMasters.AsNoTracking();

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProgramLovMast> GetProgramLovs([Service] LovDbContext db)
        => db.ProgramLovMasts.AsNoTracking();
}
