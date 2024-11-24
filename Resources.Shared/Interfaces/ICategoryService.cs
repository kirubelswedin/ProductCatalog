
using Resources.Shared.Models;

namespace Resources.Shared.Interfaces
{
    public interface ICategoryService
    {
        ResultResponse<IEnumerable<Category>> GetAllCategories();
        ResultResponse<Category> GetCategory(string nameOrId);
        ResultResponse<Category> UpdateCategory(Category category);
    }
}