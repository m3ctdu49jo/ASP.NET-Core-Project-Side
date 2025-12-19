namespace ShoppingMall.Web.Infrastructure.Services;

public interface IGenericService<TModel> where TModel : class
{
    Task<TModel> GetByIdAsync(params object[] keyValues);
    Task<IEnumerable<TModel>> GetAllAsync();
    Task AddAsync(TModel entity);
    Task UpdateAsync(TModel entity);
    Task DeleteAsync(params object[] keyValues);
    /*Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);*/
}
