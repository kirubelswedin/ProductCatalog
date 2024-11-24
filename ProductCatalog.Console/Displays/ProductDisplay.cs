using Resources.Shared.Models;

namespace ProductCatalog.Console.Displays;

public class ProductDisplay
{
    public void DisplayProducts(IEnumerable<Product> products, bool waitForKeyPress = true)
    {
        System.Console.Clear();
        System.Console.WriteLine("\n------------------ All Products -------------------");

        foreach (var product in products)
        {
            var category = product.Category.Name;
            System.Console.WriteLine($" ID: {product.Id}");
            System.Console.WriteLine($" Category: {category}");
            System.Console.WriteLine($" Product: {product.Name}, Price: {product.FormattedPrice}, Qty: {product.Quantity}");
            System.Console.WriteLine("---------------------------------------------------");
        }

        if (!waitForKeyPress) return;
        System.Console.WriteLine("\nPress any key to return to menu...");
        System.Console.ReadKey();
    }

    public void DisplayCategories(IEnumerable<Category>? categories)
    {
        if (categories?.Any() == true)
        {
            System.Console.WriteLine("\nAvailable Categories:");
            System.Console.WriteLine("---------------------------------------------------");
            foreach (var category in categories)
            {
                System.Console.WriteLine($" - {category.Name}");
            }
            System.Console.WriteLine("---------------------------------------------------");
            System.Console.WriteLine("Use an existing category or create a new one.\n");
        }
    }

    public void DisplayProductDetails(Product product)
    {
        System.Console.WriteLine("\nUpdate Product for: \n");
        System.Console.WriteLine($"Name: {product.Name}");
        System.Console.WriteLine($"Price: {product.FormattedPrice}");
        System.Console.WriteLine($"Quantity: {product.Quantity}");
        System.Console.WriteLine($"Category: {product.Category.Name}\n");
    }
}