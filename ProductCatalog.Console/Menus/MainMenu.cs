
namespace ProductCatalog.Console.Menus;

public class MainMenu : BaseMenu
{
    private readonly ProductMenu _productMenu;
    private readonly CategoryMenu _categoryMenu;

    public MainMenu(ProductMenu productMenu, CategoryMenu categoryMenu)
    {
        _productMenu = productMenu;
        _categoryMenu = categoryMenu;
    }

    protected override void DisplayMenu()
    {
        System.Console.Clear();
        System.Console.WriteLine("\n------------ Welcome to ProductCatalog ------------");
        System.Console.WriteLine("Main Menu\n---------------------------------------------------");
        System.Console.WriteLine("1. Manage Products");
        System.Console.WriteLine("2. Manage Categories");
        System.Console.WriteLine("0. Exit");
    }

    protected override bool ProcessChoice(int option)
    {
        switch (option)
        {
            case 0: 
                return true;
            case 1: 
                _productMenu.Run();
                break;
            case 2:
                _categoryMenu.Run();
                break;
            default:
                System.Console.WriteLine("Invalid option, please try again.");
                break;
        }
        return false;
    }
}