using Resources.Shared.Models;

namespace Resources.Shared.Interfaces;

public interface IProductService
{
    ResultResponse<Product> AddProduct(Product product);
    ResultResponse<IEnumerable<Product>> GetAllProducts();
    ResultResponse<Product> GetProduct(string nameOrId);  
    ResultResponse<Product> UpdateProduct(Product product);
    ResultResponse<bool> RemoveProduct(string nameOrId);
    ResultResponse<IEnumerable<Category>> GetAllCategories();
}
    
