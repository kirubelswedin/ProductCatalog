
using Resources.Shared.Factories;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace Resources.Shared.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public ResultResponse<IEnumerable<Category>> GetAllCategories()
    {
        return _categoryRepository.GetAll();
    }
    
    public ResultResponse<Category> GetCategory(string nameOrId)
    {
        return _categoryRepository.GetOne(nameOrId);
    }
    
    public ResultResponse<Category> UpdateCategory(Category category)
    {
        try
        {
            var validationResult = ValidateCategory(category);
            if (!validationResult.Success) return validationResult;

            return _categoryRepository.Update(category);
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<Category>(ex); }
    }

    // Validation
    private ResultResponse<Category> ValidateCategory(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            return ResultResponseFactory.InvalidData<Category>("Category name cannot be empty.");

        var existingCategory = _categoryRepository.GetOne(category.Id);
        if (!existingCategory.Success || existingCategory.Result == null)
            return ResultResponseFactory.NotFound<Category>("Category not found.");

        if (!existingCategory.Result.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase))
        {
            var nameExists = _categoryRepository.CategoryExists(category.Name);
            if (nameExists.Success && nameExists.Result)
                return ResultResponseFactory.Exists<Category>("A category with this name already exists.");
        }

        return ResultResponseFactory.Success<Category>(null!);
    }
}