namespace ShoppingMall.Web.Infrastructure.Services;

public interface IGenericService<TModel, TModelDTO> where TModel : class where TModelDTO : class
{
    Task<TModelDTO> GetByIdAsync(int id);
    Task<IEnumerable<TModelDTO>> GetAllAsync();
    Task AddAsync(TModelDTO entity);
    Task UpdateAsync(TModelDTO entity);
    Task DeleteAsync(int id);

    /*Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);*/
}
