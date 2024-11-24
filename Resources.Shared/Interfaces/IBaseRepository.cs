using Resources.Shared.Models;

namespace Resources.Shared.Interfaces;

public interface IBaseRepository<T>
{
    ResultResponse<T> Add(T entity);
    ResultResponse<IEnumerable<T>> GetAll();
    ResultResponse<T> GetOne(string nameOrId);
    ResultResponse<T> Update(T entity);
    ResultResponse<bool> Delete(string nameOrId);
}