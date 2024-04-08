using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Application.Services.CategoryService;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategories();

    Task<Category?> GetCategoryByIdAsync(int categoryId);
    
    Task<Category?> GetCategoryByNameAsync(string categoryName);

    Task AddCategory(Category category); 

    Task UpdateCategory(Category category);
}