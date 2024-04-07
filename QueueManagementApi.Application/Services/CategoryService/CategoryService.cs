using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;

namespace QueueManagementApi.Application.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        _categoryRepository = unitOfWork.Repository<Category>();
    }

    public async Task<List<Category>> GetAllCategories() =>
        await _categoryRepository.GetAll()
            .OrderByDescending(c => c.DisplayOrder)
            .ToListAsync();

    public async Task<Category?> GetCategoryByIdAsync(int categoryId) =>
        await _categoryRepository.FindById(categoryId);

    public async Task<Category?> GetCategoryByNameAsync(string categoryName) =>
        await _categoryRepository.FindByCondition(c => c.Name == categoryName).FirstOrDefaultAsync();

    public async Task AddCategory(Category category)
    {
        await _categoryRepository.AddAsync(category);
     
        await _unitOfWork.CompleteAsync();
    }

    public async Task UpdateCategory(Category category)
    {
        _categoryRepository.Update(category);

        await _unitOfWork.CompleteAsync();
    }
}