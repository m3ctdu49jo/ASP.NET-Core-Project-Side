
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using ShoppingMall.Infrastructure.Repositories;

namespace ShoppingMall.Infrastructure.Services;

public class GenericService<TModel, TModelDTO> : IGenericService<TModel, TModelDTO> where TModel : class where TModelDTO : class
{

    private readonly IRepository<TModel> _repository;
    private readonly IMapper _mapper;
    public GenericService(IRepository<TModel> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task AddAsync(TModelDTO entity)
    {
        var model = _mapper.Map<TModel>(entity);
        await _repository.AddAsync(model);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<TModelDTO>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<TModelDTO>>(await _repository.GetAllAsync());
    }

    public async Task<TModelDTO> GetByIdAsync(int id)
    {
        return _mapper.Map<TModelDTO>(await _repository.GetByIdAsync(id));
    }

    public async Task UpdateAsync(TModelDTO entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        var model = _mapper.Map<TModel>(entity);
        await _repository.UpdateAsync(model);
        await _repository.SaveChangesAsync();
    }
}
