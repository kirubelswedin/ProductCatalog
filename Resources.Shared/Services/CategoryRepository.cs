
using Resources.Shared.Factories;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace Resources.Shared.Services;

public class CategoryRepository : BaseRepository, ICategoryRepository
{
    public CategoryRepository(IDataPersistenceService dataPersistenceService)
        : base(dataPersistenceService)
    {
    }

    // Create
    public ResultResponse<Category> Add(Category category)
    {
        try
        {
            var loadResult = LoadCatalog();
            if (!loadResult.Success)
                return ResultResponseFactory.Failure<Category>("Failed to load product catalog.");

            var products = loadResult.Result!;
            if (products!.Any(p => p.Category.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
                return ResultResponseFactory.Exists<Category>("A category with this name already exists.");

            category.Id = Guid.NewGuid().ToString();
            return ResultResponseFactory.InvalidData<Category>("A category must be added through a product.");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<Category>(ex); }
    }

    // Read
    public ResultResponse<IEnumerable<Category>> GetAll()
    {
        try
        {
            var result = LoadCatalog();
            if (!result.Success)
                return ResultResponseFactory.Failure<IEnumerable<Category>>("Failed to load categories.");

            var categories = GetUniqueCategories(result.Result!);
            return ResultResponseFactory.Success<IEnumerable<Category>>(categories,
                "Categories retrieved successfully.");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<IEnumerable<Category>>(ex); }
    }

    public ResultResponse<Category> GetOne(string nameOrId)
    {
        try
        {
            var loadResult = LoadCatalog();
            if (!loadResult.Success)
                return ResultResponseFactory.Failure<Category>("Failed to load catalog.");

            var category = FindCategoryByNameOrId(loadResult.Result!, nameOrId);
            if (category != null)
                return ResultResponseFactory.Success(category);
            return ResultResponseFactory.NotFound<Category>($"Category with id/name '{nameOrId}' not found");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<Category>(ex); }
    }

    // Update
    public ResultResponse<Category> Update(Category category)
    {
        try
        {
            var loadResult = LoadCatalog();
            if (!loadResult.Success)
                return ResultResponseFactory.Failure<Category>("Failed to load product catalog.");

            var products = loadResult.Result!;
            
            var productsToUpdate = products
                .Where(p => p.Category.Id == category.Id ||
                            p.Category.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!productsToUpdate.Any())
                return ResultResponseFactory.NotFound<Category>("Category not found.");
            
            if (products.Any(p => p.Category.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)
                                  && p.Category.Id != category.Id))
                return ResultResponseFactory.Exists<Category>("A category with this name already exists.");
            
            foreach (var product in productsToUpdate)
            {
                product.Category = category;
            }

            var saveResult = SaveCatalog(products);
            if (!saveResult.Success)
                return ResultResponseFactory.Failure<Category>(
                    $"Failed to save category changes: {saveResult.Message}");

            return ResultResponseFactory.Success(category, "Category updated successfully.");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<Category>(ex); }
    }

    // Delete
    public ResultResponse<bool> Delete(string nameOrId)
    {
        try
        {
            var loadResult = LoadCatalog();
            if (!loadResult.Success)
                return ResultResponseFactory.Failure<bool>("Failed to load product catalog.");

            var products = loadResult.Result!;
            
            var productsWithCategory = products
                .Where(p => p.Category.Id == nameOrId ||
                            p.Category.Name.Equals(nameOrId, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!productsWithCategory.Any())
                return ResultResponseFactory.NotFound<bool>("Category not found.");
            return ResultResponseFactory.InvalidData<bool>("Cannot delete a category that is in use by products.");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<bool>(ex); }
    }

    public ResultResponse<bool> CategoryExists(string name)
    {
        return Exists(p => p.Category.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    // Helpers
    // got som help from gpt to implement these helper-methods to ease search and filtering functions.
    private List<Category> GetUniqueCategories(List<Product> products)
    {
        return
            products.Select(p => p.Category)
                .DistinctBy(c => c.Name.ToLower())
                .OrderBy(c => c.Name)
                .ToList();
    }

    private Category? FindCategoryByNameOrId(List<Product> products, string nameOrId)
    {
       return products.Select(p => p.Category)
            .FirstOrDefault(c =>
                c.Id.Equals(nameOrId, StringComparison.OrdinalIgnoreCase) ||
                c.Name.Equals(nameOrId, StringComparison.OrdinalIgnoreCase));
    }
}