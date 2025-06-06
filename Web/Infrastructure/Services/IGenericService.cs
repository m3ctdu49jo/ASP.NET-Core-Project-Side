namespace ShoppingMall.Web.Infrastructure.Services;

public interface IGenericService<TModel> where TModel : class
{
    Task<TModel> GetByIdAsync(int id);
    Task<IEnumerable<TModel>> GetAllAsync();
    Task AddAsync(TModel entity);
    Task UpdateAsync(TModel entity);
    Task DeleteAsync(int id);

    /*Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);*/
}
