using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using Document.Application.DTOs;
using Document.Infrastructure.Persistence;

namespace Document.API.GraphQL;

[QueryType]
public class DocumentQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<SignatoryDto> GetSignatories([Service] DocumentDbContext context)
        => context.Signatories
            .AsNoTracking()
            .Where(s => s.LiveFlag == "Y")
            .Select(s => new SignatoryDto(
                s.SignatoryNumber, s.Name, s.Designation,
                s.LiveFlag, s.EmployeeSysId, s.ImageFileName));

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<AppraisalLetterDto> GetAppraisalLetters([Service] DocumentDbContext context)
        => context.AppraisalLetters
            .AsNoTracking()
            .Select(l => new AppraisalLetterDto(
                l.SerialNo, l.BandCode, l.LetterType, l.FromDate, l.EndDate,
                l.Paragraph1, l.Paragraph2, l.Paragraph3, l.Paragraph4,
                l.Paragraph5, l.EffectiveDate, l.PrintDate));

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<GeneratedLetterDto> GetGeneratedLetters([Service] DocumentDbContext context)
        => context.GeneratedLetters
            .AsNoTracking()
            .Select(l => new GeneratedLetterDto(
                l.EmployeePin, l.EmployeeName, l.SignatoryName,
                l.LetterType, l.FinalRating, l.EffectiveDate, l.PrintDate));
}
