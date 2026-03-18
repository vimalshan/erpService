using MediatR;
using LovService.Application.DTOs;

namespace LovService.Application.Features.ProgramLovMast.Commands;

public record CreateProgramLovCommand(
    string PrlovTypeCode,
    string PrlovCode,
    string PrlovName) : IRequest<ProgramLovMastDto>;

public record UpdateProgramLovCommand(
    string PrlovTypeCode,
    string PrlovCode,
    string PrlovName) : IRequest<ProgramLovMastDto>;

public record DeleteProgramLovCommand(
    string PrlovTypeCode,
    string PrlovCode) : IRequest<bool>;
