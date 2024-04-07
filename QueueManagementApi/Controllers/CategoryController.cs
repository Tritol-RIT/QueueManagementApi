using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Application.Services.CategoryService;
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Controllers;

[Route("/category")]
public class CategoryController : ApiController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<List<Category>> GetCategories()
        => await _categoryService.GetAllCategories();

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategoryById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null) return NotFound();

        return Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Category>> Create(CreateCategoryDto categoryDto)
    {
        var category = new Category
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description,
            DisplayOrder = categoryDto.DisplayOrder
        };

        await _categoryService.AddCategory(category);

        return Ok(category);
    }
}