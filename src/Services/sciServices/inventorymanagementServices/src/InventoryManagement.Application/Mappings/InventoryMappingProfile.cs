using AutoMapper;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Mappings;

public sealed class InventoryMappingProfile : Profile
{
    public InventoryMappingProfile()
    {
        CreateMap<MainProductMaster, ProductDto>()
            .ConstructUsing(src => new ProductDto(
                src.ProductId,
                src.ProductName ?? string.Empty,
                src.ProductDescription,
                src.UnitId,
                src.ProductTypeId,
                src.CompanyUnitId,
                src.CreatedBy,
                src.CreatedDate,
                src.MamFlag));

        CreateMap<ItemMaster, ItemDto>()
            .ConstructUsing(src => new ItemDto(
                src.SciItemId,
                src.OracleCode,
                src.OracleItemId,
                src.MainProductId,
                src.ItemName,
                src.OracleDescription,
                src.ItemType,
                src.PackageTypeId,
                src.ItemUomId,
                src.MainProductUomConvFactor,
                src.IsBulkSource == "Y",
                src.IsBulkItem == 'Y',
                src.MaterialTaxClassId,
                src.ProductClass,
                src.EffectiveDate,
                src.ClosureDate,
                src.LeadTime,
                src.ItemCapacityId,
                src.ItemUsage));

        CreateMap<UnitOfMeasure, UnitOfMeasureDto>()
            .ConstructUsing(src => new UnitOfMeasureDto(
                src.UnitId,
                src.UnitCode,
                src.UnitOfMeasurement,
                src.UnitClassId,
                src.BaseUnitFlag,
                src.Description));

        CreateMap<ProductTypeMaster, ProductTypeDto>()
            .ConstructUsing(src => new ProductTypeDto(
                src.ProductTypeId,
                src.TypeName,
                src.TypeDescription));
    }
}
