using AutoMapper;
using UnitService.Application.DTOs;
using UnitService.Domain.Interfaces;

namespace UnitService.API.GraphQL.Queries;

public class UnitQuery
{
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<EquipmentDto>> GetEquipment(
        [Service] IUnitOfWork unitOfWork,
        [Service] IMapper mapper)
    {
        var equipment = await unitOfWork.Equipment.GetAllAsync();
        return mapper.Map<IEnumerable<EquipmentDto>>(equipment);
    }

    public async Task<EquipmentDto?> GetEquipmentById(
        int equipmentId,
        [Service] IUnitOfWork unitOfWork,
        [Service] IMapper mapper)
    {
        var equipment = await unitOfWork.Equipment.GetByIdAsync(equipmentId);
        return equipment is null ? null : mapper.Map<EquipmentDto>(equipment);
    }

    public async Task<IEnumerable<EquipmentStatusDto>> GetEquipmentStatuses(
        int equipmentId,
        [Service] IUnitOfWork unitOfWork,
        [Service] IMapper mapper)
    {
        var statuses = await unitOfWork.EquipmentStatuses.GetByEquipmentIdAsync(equipmentId);
        return mapper.Map<IEnumerable<EquipmentStatusDto>>(statuses);
    }

    public async Task<IEnumerable<CategoryDto>> GetCategories(
        [Service] IUnitOfWork unitOfWork,
        [Service] IMapper mapper)
    {
        var categories = await unitOfWork.Categories.GetAllAsync();
        return mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
}
