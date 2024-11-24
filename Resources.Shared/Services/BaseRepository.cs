
using Resources.Shared.Factories;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace Resources.Shared.Services;

public abstract class BaseRepository
{
    protected readonly IDataPersistenceService _dataPersistenceService;

    protected BaseRepository(IDataPersistenceService dataPersistenceService)
    {
        _dataPersistenceService = dataPersistenceService;
    }
    
    protected ResultResponse<List<Product>> LoadCatalog()
    {
        try
        {
            var loadResult = _dataPersistenceService.LoadCatalog();
            if (loadResult.Success)
            {
                return ResultResponseFactory.Success(loadResult.Result ?? new List<Product>(), loadResult.Message);
            } 
            return ResultResponseFactory.Failure<List<Product>>(loadResult.Message); 
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<List<Product>>(ex); }
    }
    
    protected ResultResponse<bool> SaveCatalog(List<Product> products)
    {
        try
        {
            var saveResult = _dataPersistenceService.SaveCatalog(products);
            if (saveResult.Success)
            {
                return ResultResponseFactory.Success(saveResult.Result, saveResult.Message);
            }
            return ResultResponseFactory.Failure<bool>(saveResult.Message);
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<bool>(ex); }
    }

    protected ResultResponse<Product> FindByNameOrId(string nameOrId)
    {
        try
        {
            var loadResult = LoadCatalog();
            if (!loadResult.Success)
                return ResultResponseFactory.Failure<Product>("Failed to load Catalog.");

            var product = loadResult.Result?
                .FirstOrDefault(p => p.Id == nameOrId || p.Name.Equals(nameOrId, StringComparison.OrdinalIgnoreCase));

            if (product != null)
                return ResultResponseFactory.Success(product, "Item found.");
            return ResultResponseFactory.NotFound<Product>("Item not found.");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<Product>(ex); } 
    }
    
    protected ResultResponse<bool> Exists(Func<Product, bool> predicate)
    {
        try
        {
            var loadResult = LoadCatalog();
            if (!loadResult.Success)
                return ResultResponseFactory.Failure<bool>("Failed to load catalog.");

            var exists = loadResult.Result?.Any(predicate) ?? false;
            return ResultResponseFactory.Success(exists, "Catalog loaded Successfully");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<bool>(ex); }
    }
}