using StipendService.Application.DTOs;
using StipendService.Domain.Interfaces;
using StipendService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace StipendService.API.GraphQL;

public class StipendQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<StipendMasterDto> GetStipendMasters([Service] StipendDbContext context) =>
        context.StipendMasters
            .AsNoTracking()
            .Select(m => new StipendMasterDto(
                m.Id,
                m.ResearchCategoryId,
                m.SrfRankId,
                m.SrfMonthlyStipend,
                m.AdditionalAllowance,
                m.EffectiveFrom,
                m.EffectiveTo,
                m.Status,
                m.CreatedBy,
                m.CreatedOn,
                m.UpdatedBy,
                m.UpdatedOn));

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<StipendDisbursementDto> GetDisbursements([Service] StipendDbContext context) =>
        context.StipendDisbursements
            .AsNoTracking()
            .Select(d => new StipendDisbursementDto(
                d.Id,
                d.SrfId,
                d.StipendId,
                d.DisbursementDate,
                d.DisbursementAmount,
                d.DisbursementStatus,
                d.MonthYear,
                d.BankReference,
                d.ReferenceNo,
                d.CreatedBy,
                d.CreatedOn));
}
