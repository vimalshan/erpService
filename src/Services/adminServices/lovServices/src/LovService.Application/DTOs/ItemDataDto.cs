namespace LovService.Application.DTOs;

public record ItemDataDto(int Id, string? CatName, string? ItemName, string? Make, string? Uom, int? Price);
public record CreateItemDataRequest(string? CatName, string? ItemName, string? Make, string? Uom, int? Price);
public record UpdateItemDataRequest(string? CatName, string? ItemName, string? Make, string? Uom, int? Price);
