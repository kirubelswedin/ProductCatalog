using ProductCatalog.Console.Displays;
using ProductCatalog.Console.Extensions;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace ProductCatalog.Console.Interactions;

public class ProductInteraction
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ProductDisplay _display;

    public ProductInteraction(IProductService productService, ICategoryService categoryService, ProductDisplay display)
    {
        _productService = productService;
        _categoryService = categoryService;
        _display = display;
    }

    public void CreateProduct()
    {
        var products = _productService.GetAllProducts().Result;
        if (products?.Any() == true)
        {
            _display.DisplayProducts(products, false);
        }

        while (true)
        {
            System.Console.WriteLine("\nEnter 'EXIT' to return to the menu.");
            string name = "\nEnter New Product Name: ".ReadNonEmptyString();

            if (name.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                break;

            var product = GetProductDetailsFromUser(name);
            var result = _productService.AddProduct(product);
            
            HandleOperationResult(result);
            if (result.Success)
            {
                DisplayProducts();
                break;
            }
        }
    }

    public void DisplayProducts(bool waitForKeyPress = true)
    {
        var result = _productService.GetAllProducts();
        _display.DisplayProducts(result.Result!, waitForKeyPress);
    }

    public void UpdateProduct()
    {
        while (true)
        {
            DisplayProducts(false);
            System.Console.WriteLine("\nEnter 'EXIT' to return to the menu.");
            
            string nameOrId = "\nProduct Name or ID to Update: ".ReadNonEmptyString();
            if (nameOrId.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                break;

            var existingProduct = _productService.GetProduct(nameOrId);
            if (!existingProduct.Success)
            {
                System.Console.WriteLine(existingProduct.Message);
                continue;
            }

            var updatedProduct = GetUpdatedProductDetails(existingProduct.Result!);
            var result = _productService.UpdateProduct(updatedProduct);
            
            HandleOperationResult(result);
            if (result.Success)
            {
                DisplayProducts();
                break;
            }
        }
    }

    public void DeleteProduct()
    {
        DisplayProducts(false);
        string nameOrId = "\nProduct Name or ID to Delete: ".ReadNonEmptyString();

        if (ConfirmDeletion())
        {
            var result = _productService.RemoveProduct(nameOrId);
            HandleOperationResult(result);
            if (result.Success)
            {
                DisplayProducts();
            }
        }
    }

    private Product GetProductDetailsFromUser(string name)
    {
        float price = "Enter Price: ".ReadFloat();
        int quantity = "Enter Quantity: ".ReadInt();
        
        var categories = _categoryService.GetAllCategories();
        if (categories.Success)
        {
            _display.DisplayCategories(categories.Result);
        }
        
        string categoryName = "Enter Category Name: ".ReadNonEmptyString();

        return new Product
        {
            Name = name,
            Price = price,
            Quantity = quantity,
            Category = new Category { Name = categoryName }
        };
    }

    private Product GetUpdatedProductDetails(Product current)
    {
        _display.DisplayProductDetails(current);
        
        
        string newName = "New Name: ".ReadNonEmptyString();
        float newPrice = "New Price: ".ReadFloat();
        int newQuantity = "New Quantity: ".ReadInt();
        
        var categories = _categoryService.GetAllCategories();
        if (categories.Success)
        {
            _display.DisplayCategories(categories.Result);
        }
        
        string newCategory = "New Category Name: ".ReadNonEmptyString();

        return new Product
        {
            Id = current.Id,
            Name = newName,
            Price = newPrice,
            Quantity = newQuantity,
            Category = new Category { Name = newCategory }
        };
    }

    private bool ConfirmDeletion()
    {
        System.Console.Write("Are you sure you want to Delete? (Y/N): ");
        var answer = System.Console.ReadLine();
        return answer?.Equals("Y", StringComparison.OrdinalIgnoreCase) == true;
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