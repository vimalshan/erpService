using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnitService.Application.DTOs;
using UnitService.Domain.Interfaces;

namespace UnitService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<CategoryDto>>(categories));
    }

    [HttpGet("{unitCode}")]
    public async Task<ActionResult<CategoryDto>> GetByUnitCode(string unitCode)
    {
        var category = await _unitOfWork.Categories.GetByUnitCodeAsync(unitCode);
        return category is null ? NotFound() : Ok(_mapper.Map<CategoryDto>(category));
    }
}
