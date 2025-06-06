
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using ShoppingMall.Web.Infrastructure.Repositories;

namespace ShoppingMall.Web.Infrastructure.Services;

public class GenericService<TModel> : IGenericService<TModel> where TModel : class
{

    private readonly IRepository<TModel> _repository;
    private readonly IMapper _mapper;
    public GenericService(IRepository<TModel> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task AddAsync(TModel entity)
    {
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TModel> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(TModel entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();
    }
}
