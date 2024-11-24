
using Newtonsoft.Json;
using Resources.Shared.Factories;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace Resources.Shared.Services;

public class DataPersistenceService : IDataPersistenceService
{
    private readonly IFileService _fileService;

    public DataPersistenceService(IFileService fileService)
    {
        _fileService = fileService;
    }

    public ResultResponse<List<Product>> LoadCatalog()
    {
        try
        {
            var loadResult = _fileService.GetFromFile();
            if (!loadResult.Success)
                return ResultResponseFactory.Failure<List<Product>>(loadResult.Message);

            if (string.IsNullOrEmpty(loadResult.Result))
                return ResultResponseFactory.Success(new List<Product>(), "New product catalog created.");

            var products = JsonConvert.DeserializeObject<List<Product>>(loadResult.Result);
            return ResultResponseFactory.Success(products ?? new List<Product>(), "Products loaded successfully.");
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<List<Product>>(ex); }
    }

    public ResultResponse<bool> SaveCatalog(List<Product> products)
    {
        try
        {
            var json = JsonConvert.SerializeObject(products, Formatting.Indented); 
            var saveResult = _fileService.SaveToFile(json);
            if (saveResult.Success)
                return ResultResponseFactory.Success(saveResult.Result, saveResult.Message);
                
            return ResultResponseFactory.Failure<bool>(saveResult.Message); 
        }
        catch (Exception ex)
        { return ResultResponseFactory.Exception<bool>(ex); }
    }
}
