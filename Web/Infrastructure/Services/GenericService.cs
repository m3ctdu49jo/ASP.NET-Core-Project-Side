
using Microsoft.AspNetCore.Http.HttpResults;
using ShoppingMall.Infrastructure.Repositories;

namespace ShoppingMall;

public class GenericService<T> : IGenericService<T> where T : class
{

    private readonly IRepository<T> _repository;
    public GenericService(IRepository<T> repository)
    {
        _repository = repository;
    }
    public async Task AddAsync(T entity)
    {
        await _repository.AddAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        await _repository.UpdateAsync(entity);
    }
}
