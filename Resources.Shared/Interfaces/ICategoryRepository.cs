using Resources.Shared.Models;

namespace Resources.Shared.Interfaces;

public interface ICategoryRepository : IBaseRepository<Category>
{
    ResultResponse<bool> CategoryExists(string name);
}