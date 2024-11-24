
using ProductCatalog.Console.Interactions;

namespace ProductCatalog.Console.Menus;

public class CategoryMenu : BaseMenu
{
    private readonly CategoryInteraction _interaction;

    public CategoryMenu(CategoryInteraction interaction)
    {
        _interaction = interaction;
    }

    protected override void DisplayMenu()
    {
        System.Console.Clear();
        System.Console.WriteLine("\n------------ Welcome to ProductCatalog ------------");
        System.Console.WriteLine("Category Menu\n---------------------------------------------------");
        System.Console.WriteLine("1. Display All Categories");
        System.Console.WriteLine("2. Update Category");
        System.Console.WriteLine("0. Return to Main Menu");
    }

    protected override bool ProcessChoice(int option)
    {
        switch (option)
        {
            case 0: 
                return true;
            case 1: 
                _interaction.DisplayCategories(); 
                break;
            case 2: 
                _interaction.UpdateCategory(); 
                break;
            default:
                System.Console.WriteLine("Invalid option, please try again.");
                break;
        }
        return false;
    }
}