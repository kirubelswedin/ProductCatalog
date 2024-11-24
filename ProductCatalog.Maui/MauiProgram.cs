using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ProductCatalog.Maui.ViewModels;
using ProductCatalog.Maui.Views;
using Resources.Shared.Interfaces;
using Resources.Shared.Services;
using System.Globalization;

namespace ProductCatalog.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder(); 
        builder 
            .UseMauiApp<App>() 
            .UseMauiCommunityToolkit() 
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); 
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold"); 
                fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        // Directory
        string dataDirectory = Path.Combine(FileSystem.AppDataDirectory, "Data");
        Directory.CreateDirectory(dataDirectory); 
        var filePath = Path.Combine(dataDirectory, "products.json");
 
        // Infrastructure
        builder.Services.AddSingleton<IFileService>(new FileService(filePath));
        builder.Services.AddSingleton<IDataPersistenceService, DataPersistenceService>();

        // Data
        builder.Services.AddSingleton<IProductRepository, ProductRepository>();
        builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
        
        // Business
        builder.Services.AddSingleton<IProductService, ProductService>();
        builder.Services.AddSingleton<ICategoryService, CategoryService>();

        // Presentation
        builder.Services.AddTransient<EditProductViewModel>();
        builder.Services.AddTransient<EditProductView>();

        builder.Services.AddTransient<CreateProductViewModel>();
        builder.Services.AddTransient<CreateProductView>();

        builder.Services.AddTransient<ProductListViewModel>();
        builder.Services.AddTransient<ProductListView>();
        
        // Routes
        Routing.RegisterRoute("CreateProduct", typeof(CreateProductView));
        Routing.RegisterRoute("EditProduct", typeof(EditProductView));

        return builder.Build();
    }
}