using Resources.Shared.Models;

namespace ProductCatalog.Console.Displays;

public class CategoryDisplay
{
    public void DisplayCategories(IEnumerable<Category>? categories, bool waitForKeyPress = true)
    {
        System.Console.Clear();
        System.Console.WriteLine("\n---------------- Active Categories ----------------");
        if (categories == null || !categories.Any())
        {
            System.Console.WriteLine("No active categories found.");
            System.Console.WriteLine("Categories will be created when you create products.");
            System.Console.WriteLine("---------------------------------------------------");
        }
        else
        {
            foreach (var category in categories)
            {
                System.Console.WriteLine($" ID: {category.Id}");
                System.Console.WriteLine($" Name: {category.Name}");
                System.Console.WriteLine("---------------------------------------------------");
            }
        }

        if (!waitForKeyPress) return;
        System.Console.WriteLine("\nPress any key to return to menu...");
        System.Console.ReadKey();
    }
}