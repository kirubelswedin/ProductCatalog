namespace ProductCatalog.Console.Menus;

// got some help from gpt with this BaseMenu
public abstract class BaseMenu
{
    public void Run()
    {
        while (true)
        {
            DisplayMenu();
            var option = GetUserChoice();
            if (ProcessChoice(option))
            {
                break;
            }
        }
    }
    
    protected abstract void DisplayMenu();

    private int GetUserChoice()
    {
        while (true)
        {
            System.Console.Write("\nChoose an option: ");
            var input = System.Console.ReadLine();
            if (int.TryParse(input, out int option))
            {
                return option;
            }
            System.Console.WriteLine("Invalid input, please enter a valid number.");
        }
    }
    protected abstract bool ProcessChoice(int option);
}