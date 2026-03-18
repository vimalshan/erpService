using MediatR;
using LovService.Application.DTOs;

namespace LovService.Application.Features.ProgramLovMast.Queries;

public record GetProgramLovByIdQuery(string PrlovTypeCode, string PrlovCode) : IRequest<ProgramLovMastDto?>;
public record GetAllProgramLovsQuery(string? PrlovTypeCode = null) : IRequest<IEnumerable<ProgramLovMastDto>>;
