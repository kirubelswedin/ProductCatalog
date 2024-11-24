
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductCatalog.Console.Displays;
using ProductCatalog.Console.Interactions;
using ProductCatalog.Console.Menus;
using Resources.Shared.Interfaces;
using Resources.Shared.Services;

namespace ProductCatalog.Console;

internal static class Program
{
    private static void Main(string[] args)
    {
        // Directory
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var filePath = Path.Combine(baseDirectory, "products.json");
        Debug.WriteLine($"Using file path: {filePath}");
 
        // var fileService = new FileService(productFilePath);
        // var persistDataService = new DataPersistenceService(fileService, categoryFileService);
     
        // var productList = new List<Product>();
       
        // var productService = new ProductService(persistDataService, productList);
       
        // var productViewModel = new ProductViewModel(productService);
        // var mainView = new MainMenu(categoryViewModel, productViewModel);
        
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                // Infrastructure
                services.AddSingleton<IFileService>(new FileService(filePath));
                services.AddSingleton<IDataPersistenceService, DataPersistenceService>();
                
                // Data
                services.AddSingleton<IProductRepository, ProductRepository>();
                services.AddSingleton<ICategoryRepository, CategoryRepository>();
                
                // Business
                services.AddSingleton<ICategoryService, CategoryService>();
                services.AddSingleton<IProductService, ProductService>();
                
                // Presentation - Interactions
                services.AddSingleton<ProductInteraction>();
                services.AddSingleton<CategoryInteraction>();
            
                // Presentation - Displays
                services.AddSingleton<ProductDisplay>();
                services.AddSingleton<CategoryDisplay>();
            
                // Presentation - Menus
                services.AddSingleton<ProductMenu>();
                services.AddSingleton<CategoryMenu>();
                services.AddSingleton<MainMenu>();
            })
            .Build();
        var mainView = host.Services.GetRequiredService<MainMenu>();
        mainView.Run();
    }
}

    
