using Resources.Shared.Models;

namespace Resources.Shared.Interfaces;

public interface IDataPersistenceService
{
    ResultResponse<List<Product>> LoadCatalog();
    ResultResponse<bool> SaveCatalog(List<Product> products);
}