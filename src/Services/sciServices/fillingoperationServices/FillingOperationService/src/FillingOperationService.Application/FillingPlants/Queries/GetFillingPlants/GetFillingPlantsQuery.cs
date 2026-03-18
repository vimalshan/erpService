using FillingOperationService.Application.DTOs;
using MediatR;

namespace FillingOperationService.Application.FillingPlants.Queries.GetFillingPlants;

public record GetFillingPlantsQuery(int? CompanyUnitId = null) : IRequest<IEnumerable<FillingPlantDto>>;
