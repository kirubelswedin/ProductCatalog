using Resources.Shared.Models;

namespace Resources.Shared.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    ResultResponse<bool> ProductExists(string name);
}