
using ProductCatalog.Console.Interactions;

namespace ProductCatalog.Console.Menus;

public class ProductMenu : BaseMenu
{
    private readonly ProductInteraction _interaction;
    
    public ProductMenu(ProductInteraction interaction)
    {
        _interaction = interaction;
    }

    protected override void DisplayMenu()
    {
        System.Console.Clear();
        System.Console.WriteLine("\n------------ Welcome to ProductCatalog ------------");
        System.Console.WriteLine("Product Menu\n---------------------------------------------------");
        System.Console.WriteLine("1. Add a Product");
        System.Console.WriteLine("2. Display All Products");
        System.Console.WriteLine("3. Update a Product");
        System.Console.WriteLine("4. Delete a Product");
        System.Console.WriteLine("0. Return to Main Menu");
    }

    protected override bool ProcessChoice(int option)
    {
        switch (option)
        {
            case 0: 
                return true;
            case 1: 
                _interaction.CreateProduct(); 
                break;
            case 2: 
                _interaction.DisplayProducts(); 
                break;
            case 3: 
                _interaction.UpdateProduct(); 
                break;
            case 4: 
                _interaction.DeleteProduct(); 
                break;
            default:
                System.Console.WriteLine("Invalid option, please try again.");
                break;
        }
        return false;
    }
}