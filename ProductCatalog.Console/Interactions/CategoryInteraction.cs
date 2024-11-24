using ProductCatalog.Console.Displays;
using ProductCatalog.Console.Extensions;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace ProductCatalog.Console.Interactions;

public class CategoryInteraction
{
    private readonly ICategoryService _categoryService;
    private readonly CategoryDisplay _display;

    public CategoryInteraction(ICategoryService categoryService, CategoryDisplay display)
    {
        _categoryService = categoryService;
        _display = display;
    }

    public void DisplayCategories(bool waitForKeyPress = true)
    {
        var result = _categoryService.GetAllCategories();
        _display.DisplayCategories(result.Result, waitForKeyPress);
    }

    public void UpdateCategory()
    {
        while (true)
        {
            DisplayCategories(false);
            System.Console.WriteLine("\nEnter 'EXIT' to return to the menu.");

            string nameOrId = "\nCategory Name or ID to Update: ".ReadNonEmptyString();
            if (nameOrId.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                break;

            var existingCategory = _categoryService.GetCategory(nameOrId);
            if (!existingCategory.Success)
            {
                System.Console.WriteLine(existingCategory.Message);
                continue;
            }

            var updatedCategory = GetUpdatedCategoryDetails(existingCategory.Result!);
            var result = _categoryService.UpdateCategory(updatedCategory);
            
            HandleOperationResult(result);
            if (result.Success)
            {
                DisplayCategories();
                break;
            }
        }
    }

    private Category GetUpdatedCategoryDetails(Category current)
    {
        string newName = "New Category Name: ".ReadNonEmptyString();
        return new Category
        {
            Id = current.Id,
            Name = newName
        };
    }

    private void HandleOperationResult<T>(ResultResponse<T> result)
    {
        System.Console.WriteLine(result.Message);
        if (!result.Success)
        {
            System.Console.WriteLine("Please try again.");
        }
    }
}