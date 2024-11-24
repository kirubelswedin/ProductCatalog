
using Resources.Shared.Factories;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace Resources.Shared.Services;

public class ProductRepository : BaseRepository, IProductRepository
{
    public ProductRepository(IDataPersistenceService dataPersistenceService)
    : base(dataPersistenceService)
    {
    }
    
    // Create
    public ResultResponse<Product> Add(Product product)
    {
        try
        {
            var loadResult = LoadCatalog();
            if (!loadResult.Success)
                return ResultResponseFactory.Failure<Product>("Failed to load product catalog.");

            var products = loadResult.Result ?? new List<Product>();

            product.Id = Guid.NewGuid().ToString();
            products.Add(product);

            var saveResult = SaveCatalog(products);
            if (!saveResult.Success)
                return ResultResponseFactory.Failure<Product>($"Failed to save product: {saveResult.Message}");

            return ResultResponseFactory.Success(product, "Product added successfully.");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<Product>(ex); }
    }
    
    // Read
    public ResultResponse<IEnumerable<Product>> GetAll()
    {
        try
        {
            var result = LoadCatalog();
            if (!result.Success)
                return ResultResponseFactory.Failure<IEnumerable<Product>>("Failed to load products.");
            
            return ResultResponseFactory.Success<IEnumerable<Product>>(result.Result!, "Products retrieved successfully.");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<IEnumerable<Product>>(ex); }
    }

    public ResultResponse<Product> GetOne(string nameOrId)
    {
        return FindByNameOrId(nameOrId);
    }
    
    // Update
    public ResultResponse<Product> Update(Product product)
    {
        try
        {
            var loadResult = LoadCatalog();
            if (!loadResult.Success)
                return ResultResponseFactory.Failure<Product>("Failed to load product catalog.");

            var products = loadResult.Result ?? new List<Product>();
            
            var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
                return ResultResponseFactory.NotFound<Product>("Product not found.");
            
            var index = products.IndexOf(existingProduct);
            products[index] = product;

            var saveResult = SaveCatalog(products);
            if (!saveResult.Success)
                return ResultResponseFactory.Failure<Product>($"Failed to save product: {saveResult.Message}");
            return ResultResponseFactory.Success(product, "Product updated successfully.");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<Product>(ex); }
    }

    // Delete
    public ResultResponse<bool> Delete(string nameOrId)
    {
        try
        {
            var loadResult = LoadCatalog();
            if (!loadResult.Success)
                return ResultResponseFactory.Failure<bool>("Failed to load product catalog.");

            var products = loadResult.Result ?? new List<Product>();
            
            var productToDelete = products
                .FirstOrDefault(p => p.Id == nameOrId || p.Name.Equals(nameOrId, StringComparison.OrdinalIgnoreCase));
            
            if (productToDelete == null)
                return ResultResponseFactory.NotFound<bool>("Product not found.");
            
            products.Remove(productToDelete);
            var saveResult = SaveCatalog(products);
            if (!saveResult.Success)
                return ResultResponseFactory.Failure<bool>($"Failed to save changes: {saveResult.Message}");
            return ResultResponseFactory.Success(true, "Product deleted successfully.");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<bool>(ex); }
    }
    
    public ResultResponse<bool> ProductExists(string name)
    {
        return Exists(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}