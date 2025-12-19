
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

    public async Task DeleteAsync(params object[] keyValues)
    {
        await _repository.DeleteAsync(keyValues);
        await _repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TModel> GetByIdAsync(params object[] keyValues)
    {
        return await _repository.GetByIdAsync(keyValues);
    }

    public async Task UpdateAsync(TModel entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();
    }
}
