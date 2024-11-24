
namespace ProductCatalog.Console.Extensions;

// Got help from gpt to implement this ValidationExtension
public static class ValidationExtension
{
    public static string ReadNonEmptyString(this string prompt)
    {
        string input;
        do
        {
            System.Console.Write(prompt);
            input = System.Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrEmpty(input))
            {
                System.Console.WriteLine("Input cannot be empty. Please try again.");
            }
        } while (string.IsNullOrEmpty(input));
        return input;
    }

    public static float ReadFloat(this string prompt)
    {
        float result;
        do
        {
            System.Console.Write(prompt);
            if (float.TryParse(System.Console.ReadLine(), out result))
                break;
            System.Console.WriteLine("Invalid number. Please try again.");
        } while (true);
        return result;
    }

    public static int ReadInt(this string prompt)
    {
        int result;
        do
        {
            System.Console.Write(prompt);
            if (int.TryParse(System.Console.ReadLine(), out result))
                break;
            System.Console.WriteLine("Invalid number. Please try again.");
        } while (true);
        return result;
    }
}
