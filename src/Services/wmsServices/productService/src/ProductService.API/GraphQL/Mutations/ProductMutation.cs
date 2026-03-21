using MediatR;
using ProductService.Application.Commands.CreateProduct;
using ProductService.Application.Commands.UpdateProduct;
using ProductService.Application.Commands.DeleteProduct;
using ProductService.Application.Commands.CreateCategory;
using ProductService.Application.Commands.UpdateCategory;
using ProductService.Application.Commands.DeleteCategory;
using ProductService.Application.DTOs;

namespace ProductService.API.GraphQL.Mutations;

public class ProductMutation
{
    public async Task<ProductDto> CreateProduct([Service] IMediator mediator, CreateProductDto input, CancellationToken ct)
        => await mediator.Send(new CreateProductCommand(input), ct);

    public async Task<ProductDto> UpdateProduct([Service] IMediator mediator, int productId, UpdateProductDto input, CancellationToken ct)
        => await mediator.Send(new UpdateProductCommand(productId, input), ct);

    public async Task<bool> DeleteProduct([Service] IMediator mediator, int productId, CancellationToken ct)
        => await mediator.Send(new DeleteProductCommand(productId), ct);

    public async Task<CategoryDto> CreateCategory([Service] IMediator mediator, CreateCategoryDto input, CancellationToken ct)
        => await mediator.Send(new CreateCategoryCommand(input), ct);

    public async Task<CategoryDto> UpdateCategory([Service] IMediator mediator, int categoryId, UpdateCategoryDto input, CancellationToken ct)
        => await mediator.Send(new UpdateCategoryCommand(categoryId, input), ct);

    public async Task<bool> DeleteCategory([Service] IMediator mediator, int categoryId, CancellationToken ct)
        => await mediator.Send(new DeleteCategoryCommand(categoryId), ct);
}
