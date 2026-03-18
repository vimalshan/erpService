using FillingOperationService.Application.DTOs;
using MediatR;

namespace FillingOperationService.Application.FillingPlants.Queries.GetFillingPlantById;

public record GetFillingPlantByIdQuery(int FillingPlantId) : IRequest<FillingPlantDto?>;
